[CmdletBinding()]
param(
    [string]$InstallPath = "$env:ProgramFiles\SchoolSystem",
    [switch]$CreateDesktopShortcut
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Test-IsAdministrator {
    $identity = [Security.Principal.WindowsIdentity]::GetCurrent()
    $principal = New-Object Security.Principal.WindowsPrincipal($identity)
    return $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

if (-not (Test-IsAdministrator)) {
    Write-Host 'يحتاج التثبيت إلى صلاحيات المدير حتى يكتب داخل Program Files.' -ForegroundColor Yellow
    Write-Host 'شغّل PowerShell كمسؤول ثم أعد تنفيذ السكربت.' -ForegroundColor Yellow
    exit 1
}

$sourceRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$exePath = Join-Path $sourceRoot 'SchoolSystem.exe'
$iconPath = Join-Path $sourceRoot 'Assets\Brand\SchoolSystem.ico'

if (-not (Test-Path $exePath)) {
    throw "لم يُعثر على الملف التنفيذي داخل الحزمة: $exePath"
}
if (-not (Test-Path $iconPath)) {
    throw "لم يُعثر على أيقونة النظام داخل الحزمة: $iconPath"
}

$releaseKey = 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full'
$release = 0
if (Test-Path $releaseKey) {
    $release = [int](Get-ItemPropertyValue -Path $releaseKey -Name Release -ErrorAction SilentlyContinue)
}
if ($release -lt 461808) {
    Write-Warning '.NET Framework 4.7.2 أو أحدث مطلوب لتشغيل SchoolSystem.'
    Write-Host 'ثبّت إطار العمل من Microsoft ثم أعد تشغيل هذا السكربت.' -ForegroundColor Yellow
}

if (-not (Test-Path $InstallPath)) {
    New-Item -ItemType Directory -Path $InstallPath -Force | Out-Null
}

Get-ChildItem -LiteralPath $sourceRoot -Force | Where-Object { $_.FullName -ne $InstallPath } | ForEach-Object {
    Copy-Item -LiteralPath $_.FullName -Destination $InstallPath -Recurse -Force
}

$installedExe = Join-Path $InstallPath 'SchoolSystem.exe'
$installedIcon = Join-Path $InstallPath 'Assets\Brand\SchoolSystem.ico'
$programsPath = Join-Path $env:ProgramData 'Microsoft\Windows\Start Menu\Programs'
$startMenuFolder = Join-Path $programsPath 'SchoolSystem'
New-Item -ItemType Directory -Path $startMenuFolder -Force | Out-Null

$wsh = New-Object -ComObject WScript.Shell
$startShortcut = $wsh.CreateShortcut((Join-Path $startMenuFolder 'SchoolSystem.lnk'))
$startShortcut.TargetPath = $installedExe
$startShortcut.WorkingDirectory = $InstallPath
$startShortcut.IconLocation = "$installedIcon,0"
$startShortcut.Description = 'نظام إدارة المدارس'
$startShortcut.Save()

if ($CreateDesktopShortcut) {
    $desktopShortcut = $wsh.CreateShortcut((Join-Path ([Environment]::GetFolderPath('Desktop')) 'SchoolSystem.lnk'))
    $desktopShortcut.TargetPath = $installedExe
    $desktopShortcut.WorkingDirectory = $InstallPath
    $desktopShortcut.IconLocation = "$installedIcon,0"
    $desktopShortcut.Description = 'نظام إدارة المدارس'
    $desktopShortcut.Save()
}

Write-Host "تم تثبيت SchoolSystem في: $InstallPath" -ForegroundColor Green
Write-Host 'يمكن تشغيله من قائمة ابدأ أو من الملف SchoolSystem.exe.'
if (-not $CreateDesktopShortcut) {
    Write-Host 'لإنشاء اختصار على سطح المكتب أعد التشغيل مع -CreateDesktopShortcut.'
}
