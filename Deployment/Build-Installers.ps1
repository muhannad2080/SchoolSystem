[CmdletBinding()]
param(
    [string]$Configuration = 'Release',
    [string]$Version = '1.0.0'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'SchoolSystem.sln'
$buildOutput = Join-Path $root "bin\$Configuration"
$distRoot = Join-Path $root 'dist'
$bootstrapperRoot = Join-Path $PSScriptRoot 'Bootstrapper'
$bootstrapperProject = Join-Path $bootstrapperRoot 'SchoolSystem.SetupBootstrapper.csproj'
$payloadZip = Join-Path $bootstrapperRoot 'Payload.zip'
$payloadStage = Join-Path $bootstrapperRoot 'PayloadStage'
$bootstrapperOutput = Join-Path $bootstrapperRoot "bin\$Configuration\SchoolSystem-SetupBootstrapper.exe"
$exeOut = Join-Path $distRoot "SchoolSystem-$Version-Setup.exe"
$shaOut = "$exeOut.sha256"

function Resolve-MSBuild {
    $candidates = @(
        'C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe'
    )

    $command = Get-Command msbuild.exe -ErrorAction SilentlyContinue
    if ($command) {
        $candidates += $command.Source
    }

    foreach ($candidate in $candidates) {
        if ($candidate -and (Test-Path $candidate)) {
            return $candidate
        }
    }

    throw 'لم يتم العثور على MSBuild. شغّل PowerShell من Developer PowerShell/Developer Command Prompt الخاص بـ Visual Studio 2022.'
}

function Assert-RequiredFile([string]$relativePath) {
    $path = Join-Path $buildOutput $relativePath
    if (-not (Test-Path $path)) {
        throw "ملف الإصدار المطلوب غير موجود: $path"
    }
}

function Copy-Payload {
    if (Test-Path $payloadStage) {
        Remove-Item $payloadStage -Recurse -Force
    }
    if (Test-Path $payloadZip) {
        Remove-Item $payloadZip -Force
    }

    New-Item -ItemType Directory -Path $payloadStage -Force | Out-Null

    $files = Get-ChildItem -LiteralPath $buildOutput -Recurse -File |
        Where-Object { $_.Name -notlike '*.pdb' -and $_.Extension -notmatch '^\.bak$' }

    if (-not $files -or $files.Count -eq 0) {
        throw 'لم توجد ملفات تشغيل داخل مخرجات Release.'
    }

    foreach ($file in $files) {
        $relative = $file.FullName.Substring($buildOutput.Length).TrimStart('\', '/')
        $target = Join-Path $payloadStage $relative
        $targetDirectory = Split-Path -Parent $target

        if (-not (Test-Path $targetDirectory)) {
            New-Item -ItemType Directory -Path $targetDirectory -Force | Out-Null
        }

        Copy-Item -LiteralPath $file.FullName -Destination $target -Force
    }

    $docsSource = Join-Path $root 'docs'
    if (Test-Path $docsSource) {
        $docsTarget = Join-Path $payloadStage 'Documentation'
        New-Item -ItemType Directory -Path $docsTarget -Force | Out-Null

        Get-ChildItem -LiteralPath $docsSource -File |
            Where-Object { $_.Extension -in @('.md', '.txt', '.pdf') } |
            ForEach-Object {
                Copy-Item -LiteralPath $_.FullName -Destination $docsTarget -Force
            }
    }

    $readme = @"
SchoolSystem $Version

هذه حزمة تثبيت Windows ذاتية الاستخراج.
تحتوي الحزمة على البرنامج، مكتبات التشغيل، مخطط قاعدة البيانات، migrations، وبيانات المرجع الأكاديمي.

عند التشغيل الأول:
- إذا كانت SchoolDB موجودة: يستخدمها النظام ويحافظ على بياناتها ويطبق الترحيلات الناقصة فقط.
- إذا لم تكن SchoolDB موجودة: ينشئها تلقائيًا من databaseschool.sql ثم يطبق migrations وبيانات المرجع.
- لا يتم حذف قاعدة موجودة تلقائيًا.

المتطلبات:
- Windows 10/11.
- .NET Framework 4.7.2 أو أحدث.
- SQL Server متاح محليًا أو عبر الخادم المحدد.
- صلاحيات المدير مطلوبة لتثبيت البرنامج في Program Files.
"@

    Set-Content -LiteralPath (Join-Path $payloadStage 'README-تشغيل.txt') -Value $readme -Encoding UTF8

    Compress-Archive -Path (Join-Path $payloadStage '*') -DestinationPath $payloadZip -CompressionLevel Optimal

    if (-not (Test-Path $payloadZip)) {
        throw "فشل إنشاء الحمولة: $payloadZip"
    }
}

New-Item -ItemType Directory -Path $distRoot -Force | Out-Null

if (-not (Test-Path $solution)) {
    throw "ملف الحل غير موجود: $solution"
}
if (-not (Test-Path $bootstrapperProject)) {
    throw "مشروع Bootstrapper غير موجود: $bootstrapperProject"
}

$msbuild = Resolve-MSBuild

Write-Host '============================================================'
Write-Host ' SchoolSystem - بناء حزمة التثبيت النهائية'
Write-Host " Configuration: $Configuration"
Write-Host " Version:       $Version"
Write-Host '============================================================'

Write-Host ''
Write-Host '[1/5] إعادة بناء Release...'
& $msbuild $solution /t:Rebuild "/p:Configuration=$Configuration" "/p:Platform=Any CPU" /m /v:minimal /nologo
if ($LASTEXITCODE -ne 0) {
    throw "فشل بناء SchoolSystem. رمز الخروج: $LASTEXITCODE"
}

Write-Host ''
Write-Host '[2/5] فحص ملفات التشغيل وقاعدة البيانات...'
Assert-RequiredFile 'SchoolSystem.exe'
Assert-RequiredFile 'SchoolSystem.exe.config'
Assert-RequiredFile 'iTextSharp.LGPLv2.Core.dll'
Assert-RequiredFile 'Database\databaseschool.sql'
Assert-RequiredFile 'Database\SeedAcademicStructure_2026_2027.sql'

$migrationDirectory = Join-Path $buildOutput 'Database\migrations'
$migrationFiles = Get-ChildItem -LiteralPath $migrationDirectory -Filter '*.sql' -File -ErrorAction SilentlyContinue
if (-not $migrationFiles -or $migrationFiles.Count -eq 0) {
    throw "لا توجد migrations داخل: $migrationDirectory"
}

Write-Host ("تم العثور على {0} migration" -f $migrationFiles.Count)

Write-Host ''
Write-Host '[3/5] إنشاء الحمولة المضمنة...'
Copy-Payload

Write-Host ''
Write-Host '[4/5] بناء Setup.exe من Bootstrapper المحلي...'
& $msbuild $bootstrapperProject /t:Rebuild "/p:Configuration=$Configuration" "/p:Platform=AnyCPU" "/p:SetupVersion=$Version" "/p:PayloadZip=$payloadZip" /m /v:minimal /nologo
if ($LASTEXITCODE -ne 0) {
    throw "فشل بناء Setup Bootstrapper. رمز الخروج: $LASTEXITCODE"
}

if (-not (Test-Path $bootstrapperOutput)) {
    throw "لم ينتج Bootstrapper ملف Setup.exe: $bootstrapperOutput"
}

if (Test-Path $exeOut) {
    Remove-Item $exeOut -Force
}

Copy-Item -LiteralPath $bootstrapperOutput -Destination $exeOut -Force

$hash = (Get-FileHash -LiteralPath $exeOut -Algorithm SHA256).Hash
Set-Content -LiteralPath $shaOut -Value "$hash  $(Split-Path $exeOut -Leaf)" -Encoding ASCII

Write-Host ''
Write-Host '[5/5] تنظيف ملفات البناء المؤقتة...'
if (Test-Path $payloadZip) {
    Remove-Item $payloadZip -Force
}
if (Test-Path $payloadStage) {
    Remove-Item $payloadStage -Recurse -Force
}

Write-Host ''
Write-Host '============================================================'
Write-Host ' تم تجهيز حزمة التثبيت:'
Write-Host " $exeOut"
Write-Host ''
Write-Host " SHA-256: $hash"
Write-Host ''
Write-Host 'لا يحتاج بناء Setup.exe إلى Inno Setup أو winget.'
Write-Host 'الملف النهائي يحتوي البرنامج + DLLs + Database + migrations + seed.'
Write-Host '============================================================'
