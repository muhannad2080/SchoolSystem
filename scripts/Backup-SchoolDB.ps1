[CmdletBinding()]
param(
    [string]$ServerInstance = "localhost",
    [string]$Database = "SchoolDB",
    [string]$BackupDirectory = "C:\SchoolSystemBackups",
    [string]$SqlCredentialFile = ""
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($ServerInstance)) { throw "يجب تحديد اسم خادم SQL Server." }
if ([string]::IsNullOrWhiteSpace($Database) -or $Database -notmatch '^[A-Za-z0-9_\-]+$') { throw "اسم قاعدة البيانات غير صالح." }
if ([string]::IsNullOrWhiteSpace($BackupDirectory)) { throw "يجب تحديد مجلد النسخ الاحتياطي." }

$resolvedDirectory = [System.IO.Path]::GetFullPath($BackupDirectory)
$repositoryPath = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))
if ($resolvedDirectory.StartsWith($repositoryPath, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "يجب حفظ النسخة الاحتياطية خارج مجلد المستودع." 
}

New-Item -ItemType Directory -Path $resolvedDirectory -Force | Out-Null
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$backupFile = Join-Path $resolvedDirectory ("{0}_{1}.bak" -f $Database, $timestamp)

$connectionString = "Server=$ServerInstance;Database=master;Integrated Security=True;TrustServerCertificate=True;"
if (-not [string]::IsNullOrWhiteSpace($SqlCredentialFile)) {
    if (-not (Test-Path -LiteralPath $SqlCredentialFile)) { throw "ملف بيانات الاعتماد غير موجود." }
    $credential = Import-Clixml -LiteralPath $SqlCredentialFile
    $connectionString = "Server=$ServerInstance;Database=master;User ID=$($credential.UserName);Password=$($credential.GetNetworkCredential().Password);TrustServerCertificate=True;"
}

Add-Type -AssemblyName System.Data
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$command = $connection.CreateCommand()
$command.CommandTimeout = 0
$command.CommandText = "BACKUP DATABASE [$Database] TO DISK = @backupFile WITH CHECKSUM, INIT, STATS = 10;"
$command.Parameters.Add("@backupFile", [System.Data.SqlDbType]::NVarChar, 4000).Value = $backupFile

try {
    $connection.Open()
    [void]$command.ExecuteNonQuery()
    Write-Host "تم إنشاء النسخة الاحتياطية: $backupFile"
}
finally {
    if ($connection.State -ne [System.Data.ConnectionState]::Closed) { $connection.Close() }
    $connection.Dispose()
}
