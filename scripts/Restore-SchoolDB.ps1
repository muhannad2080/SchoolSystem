[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = "High")]
param(
    [Parameter(Mandatory = $true)]
    [string]$ServerInstance,
    [Parameter(Mandatory = $true)]
    [string]$BackupFile,
    [string]$TargetDatabase = "SchoolDB_RestoreTest",
    [switch]$ReplaceExisting
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $BackupFile -PathType Leaf)) { throw "ملف النسخة الاحتياطية غير موجود." }
if ([string]::IsNullOrWhiteSpace($TargetDatabase) -or $TargetDatabase -notmatch '^[A-Za-z0-9_\-]+$') { throw "اسم قاعدة البيانات الهدف غير صالح." }

$backupFullPath = [System.IO.Path]::GetFullPath($BackupFile)
$connectionString = "Server=$ServerInstance;Database=master;Integrated Security=True;TrustServerCertificate=True;"

Add-Type -AssemblyName System.Data
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$command = $connection.CreateCommand()
$command.CommandTimeout = 0

$restoreMode = if ($ReplaceExisting) { "REPLACE" } else { "NORECOVERY" }
if ($ReplaceExisting) {
    $command.CommandText = @"
IF DB_ID(@targetDatabase) IS NOT NULL
BEGIN
    ALTER DATABASE [$TargetDatabase] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
END;
RESTORE DATABASE [$TargetDatabase]
FROM DISK = @backupFile
WITH REPLACE, RECOVERY, CHECKSUM, STATS = 10;
ALTER DATABASE [$TargetDatabase] SET MULTI_USER;
"@
} else {
    $command.CommandText = @"
IF DB_ID(@targetDatabase) IS NOT NULL
    THROW 50001, 'قاعدة البيانات الهدف موجودة. استخدم -ReplaceExisting بعد أخذ نسخة احتياطية منها.', 1;
RESTORE DATABASE [$TargetDatabase]
FROM DISK = @backupFile
WITH RECOVERY, CHECKSUM, STATS = 10;
"@
}

[void]$command.Parameters.Add("@targetDatabase", [System.Data.SqlDbType]::NVarChar, 128)
$command.Parameters["@targetDatabase"].Value = $TargetDatabase
[void]$command.Parameters.Add("@backupFile", [System.Data.SqlDbType]::NVarChar, 4000)
$command.Parameters["@backupFile"].Value = $backupFullPath

if ($PSCmdlet.ShouldProcess("$ServerInstance/$TargetDatabase", "استعادة $backupFullPath")) {
    try {
        $connection.Open()
        [void]$command.ExecuteNonQuery()
        Write-Host "تمت استعادة النسخة إلى قاعدة: $TargetDatabase"
    }
    finally {
        if ($connection.State -ne [System.Data.ConnectionState]::Closed) { $connection.Close() }
        $connection.Dispose()
    }
}
