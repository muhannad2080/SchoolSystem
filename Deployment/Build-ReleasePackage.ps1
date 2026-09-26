[CmdletBinding()]
param(
    [string]$Configuration = 'Release',
    [string]$Platform = 'Any CPU',
    [string]$Version = '1.0.0'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'SchoolSystem.sln'
$buildOutput = Join-Path $root "bin\$Configuration"
$distRoot = Join-Path $root 'dist'
$packageName = "SchoolSystem-$Version"
$packageRoot = Join-Path $distRoot $packageName
$zipPath = Join-Path $distRoot "$packageName.zip"

if (-not (Test-Path $solution)) {
    throw "لم يتم العثور على ملف الحل: $solution"
}

$msbuildCommand = Get-Command msbuild.exe -ErrorAction SilentlyContinue
if (-not $msbuildCommand) {
    $msbuildCommand = Get-Command msbuild -ErrorAction SilentlyContinue
}
if (-not $msbuildCommand) {
    throw 'شغّل السكربت من Developer PowerShell الخاص بـ Visual Studio أو أضف MSBuild إلى PATH.'
}

if (Test-Path $packageRoot) {
    Remove-Item $packageRoot -Recurse -Force
}
if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}
New-Item -ItemType Directory -Path $packageRoot -Force | Out-Null

& $msbuildCommand.Source $solution /t:Rebuild /p:Configuration=$Configuration "/p:Platform=$Platform" /m
if ($LASTEXITCODE -ne 0) {
    throw "فشل بناء الحل برمز الخروج $LASTEXITCODE"
}
if (-not (Test-Path (Join-Path $buildOutput 'SchoolSystem.exe'))) {
    throw "لم يُنتج البناء الملف التنفيذي في: $buildOutput"
}

$buildItems = Get-ChildItem -LiteralPath $buildOutput -Force | Where-Object { $_.Name -ne '%(Dir)%(Filename)%(Extension)' }
foreach ($buildItem in $buildItems) {
    Copy-Item -LiteralPath $buildItem.FullName -Destination $packageRoot -Recurse -Force
}

$assetRoot = Join-Path $root 'Assets'
$packageAssetRoot = Join-Path $packageRoot 'Assets'
New-Item -ItemType Directory -Path $packageAssetRoot -Force | Out-Null
Copy-Item (Join-Path $assetRoot 'Brand\SchoolSystem.ico') (Join-Path $packageAssetRoot 'Brand\SchoolSystem.ico') -Force
if (Test-Path (Join-Path $assetRoot 'Welcome\welcome_hero.png')) {
    New-Item -ItemType Directory -Path (Join-Path $packageAssetRoot 'Welcome') -Force | Out-Null
    Copy-Item (Join-Path $assetRoot 'Welcome\welcome_hero.png') (Join-Path $packageAssetRoot 'Welcome\welcome_hero.png') -Force
}

Copy-Item (Join-Path $PSScriptRoot 'Install-SchoolSystem.ps1') $packageRoot -Force

# قاعدة البيانات تُحدَّث تلقائياً عند أول تشغيل عبر محرك الترحيل المدمج
# (Database\initial + Database\migrations)، وقد نُسخت مع مخرجات البناء أعلاه.
# تُوضع سكربتات البرنامج الخام المرجعية في مجلد منفصل لا يُستخدَم للتشغيل.
$legacyScriptsTarget = Join-Path $packageRoot 'Database\Legacy-Reference'
New-Item -ItemType Directory -Path $legacyScriptsTarget -Force | Out-Null
foreach ($referenceFile in @('SchoolDB_Schema.sql', 'SchoolDB_OperationalData.sql')) {
    $databaseSource = Join-Path $root "Databass\$referenceFile"
    if (Test-Path $databaseSource) {
        Copy-Item -LiteralPath $databaseSource -Destination $legacyScriptsTarget -Force
    }
}

$docsTarget = Join-Path $packageRoot 'Documentation'
New-Item -ItemType Directory -Path $docsTarget -Force | Out-Null
Copy-Item (Join-Path $root 'docs\Deployment_and_Operations_Guide_AR.md') $docsTarget -Force
Copy-Item (Join-Path $root 'docs\Windows_Release_Package_Guide_AR.md') $docsTarget -Force
Copy-Item (Join-Path $root 'docs\Database_Script_Catalog_AR.md') $docsTarget -Force
Copy-Item (Join-Path $root 'docs\Database_Scripts_Complete_Runbook_AR.md') $docsTarget -Force

$skiaRuntimeRoot = Join-Path $root 'packages\SkiaSharp.NativeAssets.Win32.4.148.0\runtimes'
foreach ($runtime in @('win-x86', 'win-x64', 'win-arm64')) {
    $nativeSource = Join-Path $skiaRuntimeRoot "$runtime\native\libSkiaSharp.dll"
    if (Test-Path $nativeSource) {
        $nativeTarget = Join-Path $packageRoot "runtimes\$runtime\native"
        New-Item -ItemType Directory -Path $nativeTarget -Force | Out-Null
        Copy-Item $nativeSource (Join-Path $nativeTarget 'libSkiaSharp.dll') -Force
    }
}

$readme = @"
SchoolSystem $Version

ابدأ بقراءة Documentation\Windows_Release_Package_Guide_AR.md.
ينشئ البرنامج قاعدة البيانات ويحدّثها تلقائياً عند أول تشغيل عبر محرك الترحيل المدمج:
  Database\initial\001_InitialSchema.sql  (المخطط الأولي)
  Database\migrations\*.sql               (ترحيلات مرقّمة تلقائية)
الفهرس المرجعي للقاعدة السابق موجود في Database\Legacy-Reference\ ولا يُستخدَم للتشغيل.
الملف التنفيذي: SchoolSystem.exe
ملف الإعدادات: SchoolSystem.exe.config
الأيقونة: Assets\Brand\SchoolSystem.ico

هذه حزمة تشغيل محمولة. لا حاجة لتشغيل أي سكربت يدوي: عند أول تشغيل تُنشأ قاعدة
SchoolDB (أو تُعتمَد قاعدة موجودة كأساس بلا فقد بيانات) وتُلحق الترحيلات المرقّمة تلقائياً.
الشروط المسبقة: تثبيت .NET Framework 4.7.2 أو أحدث وتجهيز SQL Server محلي
(Windows Authentication أو حساب SQL). ضبط الاتصال من شاشة الإعدادات داخل البرنامج.
"@
Set-Content -Path (Join-Path $packageRoot 'README-تشغيل.txt') -Value $readme -Encoding UTF8

Compress-Archive -Path (Join-Path $packageRoot '*') -DestinationPath $zipPath -CompressionLevel Optimal
$hash = (Get-FileHash $zipPath -Algorithm SHA256).Hash
Set-Content -Path (Join-Path $distRoot "$packageName.sha256") -Value "$hash  $packageName.zip" -Encoding ASCII

Write-Host "تم إنشاء الحزمة: $zipPath"
Write-Host "SHA-256: $hash"
