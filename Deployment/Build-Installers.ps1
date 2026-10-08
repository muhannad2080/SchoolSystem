[CmdletBinding()]
param(
    [string]$Configuration = 'Release',
    [string]$Version = '1.0.0',
    [switch]$BuildMsi
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$buildOutput = Join-Path $root "bin\$Configuration"
$distRoot = Join-Path $root 'dist'
$installerDir = Join-Path $PSScriptRoot 'installer'
$wxsPath = Join-Path $installerDir 'SchoolSystem.generated.wxs'
$msiPath = Join-Path $distRoot "SchoolSystem-$Version.msi"
$exeOut = Join-Path $distRoot "SchoolSystem-Setup-$Version.exe"

New-Item -ItemType Directory -Path $distRoot -Force | Out-Null

# وظيفة توليد GUID ثابت (محدّد) من أي نص كي تبقى مكوّنات الترقية مستقرة.
function Convert-ComponentGuid([string]$key) {
    $md5 = [System.Security.Cryptography.MD5]::Create()
    $hash = $md5.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($key.ToLowerInvariant()))
    $bytes = New-Object byte[] 16
    [Array]::Copy($hash, $bytes, 16)
    $bytes[7] = (($bytes[7] -band 0x0F) -bor 0x30)   # version 3 (name-based)
    $bytes[8] = (($bytes[8] -band 0x3F) -bor 0x80)   # RFC 4122 variant
    return (New-Object Guid -ArgumentList (,$bytes)).ToString().ToUpperInvariant()
}

# ---------------------------------------------------------------------------
# اكتشاف أدوات البناء
# ---------------------------------------------------------------------------
$wixExe = $null
if ($BuildMsi) {
    foreach ($candidate in @(
            'C:\Program Files\WiX Toolset v7.0\bin\wix.exe',
            'C:\Program Files (x86)\WiX Toolset v7.0\bin\wix.exe',
            (Join-Path $env:USERPROFILE '.dotnet\tools\wix.exe'),
            (Get-Command wix.exe -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source))) {
        if ($candidate -and (Test-Path $candidate)) { $wixExe = $candidate; break }
    }
    if (-not $wixExe) {
        throw 'لم يتم العثور على wix.exe. بناء Setup.exe لا يحتاج WiX؛ استخدم -BuildMsi فقط عند الحاجة إلى MSI.'
    }
}

$isccExe = $null
foreach ($candidate in @(
        "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe",
        'C:\Program Files (x86)\Inno Setup 6\ISCC.exe',
        (Get-Command ISCC.exe -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source))) {
    if ($candidate -and (Test-Path $candidate)) { $isccExe = $candidate; break }
}
if (-not $isccExe) {
    throw 'لم يتم العثور على ISCC.exe (Inno Setup). ثبّته عبر winget: winget install JRSoftware.InnoSetup'
}

# ---------------------------------------------------------------------------
# إعادة بناء إصدار Release من المصدر ليكون في الملفات أحدث نسخة
# ---------------------------------------------------------------------------
$msbuildCandidates = @(
    'C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe',
    'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe',
    'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe',
    (Get-Command msbuild.exe -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source))
$msbuildExe = $msbuildCandidates | Where-Object { $_ -and (Test-Path $_) } | Select-Object -First 1
if ($msbuildExe) {
    & $msbuildExe (Join-Path $root 'SchoolSystem.sln') /p:Configuration=$Configuration "/p:Platform=Any CPU" /v:minimal /nologo
    if ($LASTEXITCODE -ne 0) { throw 'فشل بناء حل SchoolSystem.' }
} else {
    Write-Warning 'لم يتم العثور على MSBuild؛ سنبني من مخرجات bin\Release الحالية دون إعادة بناء.'
}
if (-not (Test-Path (Join-Path $buildOutput 'SchoolSystem.exe'))) {
    throw "لم يُعثر على SchoolSystem.exe في: $buildOutput"
}

foreach ($requiredRelativePath in @(
    'SchoolSystem.exe',
    'SchoolSystem.exe.config',
    'iTextSharp.LGPLv2.Core.dll',
    'Database\databaseschool.sql',
    'Database\SeedAcademicStructure_2026_2027.sql'
)) {
    $requiredPath = Join-Path $buildOutput $requiredRelativePath
    if (-not (Test-Path $requiredPath)) {
        throw "ملف التثبيت المطلوب غير موجود بعد البناء: $requiredPath"
    }
}

$migrationFiles = Get-ChildItem -LiteralPath (Join-Path $buildOutput 'Database\migrations') -Filter '*.sql' -File -ErrorAction SilentlyContinue
if (-not $migrationFiles -or $migrationFiles.Count -eq 0) {
    throw 'لم توجد ترحيلات قاعدة البيانات داخل مخرجات Release؛ لن نبني حزمة تثبيت ناقصة.'
}

if ($BuildMsi) {
# ---------------------------------------------------------------------------
# 1) بناء MSI عبر WiX v7 (توليد WXS من محتويات bin\Release)
# ---------------------------------------------------------------------------
$files = Get-ChildItem -LiteralPath $buildOutput -Recurse -File
if (-not $files -or $files.Count -eq 0) {
    throw 'مجلد الإخراج فارغ؛ لا يمكن بناء الحزمة.'
}

# GUID ثابت لترقية صاعدة آمنة
$upgradeCode = '9F5C7E62-1A2B-4C3D-8E4F-5A6B7C8D9E0F'

$singleLine = @'
<Wix xmlns="http://wixtoolset.org/schemas/v4/wxs">
  <Package Name="نظام إدارة المدارس - SchoolSystem" Manufacturer="SchoolSystem"
           Version="{0}" UpgradeCode="{1}" Language="1025" Codepage="65001" Scope="perMachine">
    <MajorUpgrade DowngradeErrorMessage="يوجد إصدار أحدث من SchoolSystem مثبّت بالفعل." />
    <MediaTemplate EmbedCab="yes" CompressionLevel="high" />
    <StandardDirectory Id="ProgramFilesFolder">
      <Directory Id="INSTALLFOLDER" Name="SchoolSystem">
{2}
      </Directory>
    </StandardDirectory>
    <StandardDirectory Id="ProgramMenuFolder">
      <Directory Id="ProgramMenuDir" Name="SchoolSystem">
        <Component Id="cm_startmenu" Guid="{3}">
          <Shortcut Id="startShortcut" Name="SchoolSystem" Description="نظام إدارة المدارس"
                    Target="[INSTALLFOLDER]SchoolSystem.exe" WorkingDirectory="INSTALLFOLDER"
                    Icon="SchoolSystem.ico" />
          <RemoveFolder Id="RemoveProgramMenuDir" Directory="ProgramMenuDir" On="uninstall" />
          <RegistryValue Root="HKMU" Key="Software\SchoolSystem" Name="installed"
                         Type="integer" Value="1" KeyPath="yes" />
        </Component>
      </Directory>
    </StandardDirectory>
    <StandardDirectory Id="DesktopFolder">
      <Component Id="cm_desktop" Guid="{4}">
        <Shortcut Id="desktopShortcut" Name="SchoolSystem" Description="نظام إدارة المدارس"
                  Target="[INSTALLFOLDER]SchoolSystem.exe" WorkingDirectory="INSTALLFOLDER"
                  Icon="SchoolSystem.ico" />
        <RegistryValue Root="HKMU" Key="Software\SchoolSystem" Name="desktopShortcut"
                       Type="integer" Value="1" KeyPath="yes" />
      </Component>
    </StandardDirectory>
    <Icon Id="SchoolSystem.ico" SourceFile="{5}" />
  </Package>
</Wix>
'@

# نسّق اسم الملف الداخلي: نسبةً إلى INSTALLFOLDER
$base = [System.IO.Path]::GetFullPath($buildOutput).TrimEnd('\')
$sb = New-Object System.Text.StringBuilder

foreach ($file in $files) {
    $rel = $file.FullName.Substring($base.Length).TrimStart('\')
    if ($file.Name -like '*.pdb') { continue }            # نتجاهل رموز التصحيح
    if ($rel -like 'runtimes\*') { continue }              # تُعالج libSkiaSharp يدوياً أدناه

    # مكوّن مستقل لكل ملف كي يدعم الترقية. اسم فريد مبني من المسار.
    $compId = 'cmp_' + (($rel -replace '[^A-Za-z0-9]', '_'))
    $compGuid = Convert-ComponentGuid $rel

    # مسار قاعدة بيانات WiX v7: قطرات داخل INSTALLFOLDER
    $source = $file.FullName

    if ($rel -eq 'x86\libSkiaSharp.dll' -or $rel -eq 'x64\libSkiaSharp.dll' -or $rel -eq 'arm64\libSkiaSharp.dll') {
        [void]$sb.AppendLine("        <Component Id='$compId' Guid='$compGuid' Bitness='always64'>")
    } else {
        [void]$sb.AppendLine("        <Component Id='$compId' Guid='$compGuid'>")
    }
    [void]$sb.AppendLine("          <File Id='fil_$compId' Source='$source' Name='$($file.Name)' />")
    [void]$sb.AppendLine("        </Component>")
}

$wxs = $singleLine -f $Version, $upgradeCode, $sb.ToString(),
    (Convert-ComponentGuid 'ProgramMenuDir'),
    (Convert-ComponentGuid 'DesktopShortcut'),
    (Join-Path $root 'Assets\Brand\SchoolSystem.ico')

# حفظ WXS بترميز UTF-8 (الملف يحتوي نصاً عربياً في حقل الاسم)
[System.IO.File]::WriteAllText($wxsPath, $wxs, (New-Object System.Text.UTF8Encoding($true)))

Write-Host "بناء MSI عبر WiX v7 ..."
& $wixExe build $wxsPath -o $msiPath -arch x64 -ext WixToolset.UI.wixext -acceptEula wix7
if ($LASTEXITCODE -ne 0 -and -not (Test-Path $msiPath)) {
    throw 'فشل بناء MSI بـ WiX.'
}
Write-Host "تم إنشاء MSI: $msiPath"


}
# ---------------------------------------------------------------------------
# 2) بناء EXE عبر Inno Setup
# ---------------------------------------------------------------------------
Write-Host "بناء Setup.exe عبر Inno Setup ..."
& $isccExe (Join-Path $installerDir 'SchoolSystem.iss') "/DMyAppVersion=$Version"
if ($LASTEXITCODE -ne 0 -and -not (Test-Path $exeOut)) {
    throw 'فشل بناء Setup.exe بـ Inno.'
}
Write-Host "تم إنشاء EXE: $exeOut"

# ---------------------------------------------------------------------------
# تواقيع SHA-256
# ---------------------------------------------------------------------------
foreach ($artifact in @($msiPath, $exeOut)) {
    if (Test-Path $artifact) {
        $hash = (Get-FileHash $artifact -Algorithm SHA256).Hash
        $shaFile = "$artifact.sha256"
        Set-Content -Path $shaFile -Value "$hash  $(Split-Path $artifact -Leaf)" -Encoding ASCII
        Write-Host ("SHA-256 [{0}]: {1}" -f (Split-Path $artifact -Leaf), $hash)
    }
}

Write-Host ''
Write-Host "اكتمل بناء حزمة التثبيت الأساسية:"
Write-Host "  Setup EXE: $exeOut"
if ($BuildMsi) {
    Write-Host "  MSI: $msiPath"
}
Write-Host "  قاعدة البيانات والـmigrations والبيانات المرجعية مضمّنة داخل Setup.exe."