$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'SchoolSystem.sln'
$packages = Join-Path $root 'packages'
$nuget = Join-Path $root 'nuget.exe'

if (-not (Test-Path $solution)) {
    throw "لم يتم العثور على SchoolSystem.sln في $root"
}

if (-not (Test-Path $nuget)) {
    $nugetCommand = Get-Command nuget.exe -ErrorAction SilentlyContinue
    if ($null -eq $nugetCommand) {
        throw "لم يتم العثور على nuget.exe. ثبّت NuGet CLI أو ضع nuget.exe في مجلد المشروع."
    }
    $nuget = $nugetCommand.Source
}

Write-Host "استعادة حزم NuGet إلى: $packages" -ForegroundColor Cyan
& $nuget restore $solution -PackagesDirectory $packages -NonInteractive
if ($LASTEXITCODE -ne 0) {
    throw "فشلت استعادة حزم NuGet. تحقق من اتصال الإنترنت ومصدر nuget.org."
}

Write-Host "تمت استعادة الحزم بنجاح. أغلق الحل وأعد فتح SchoolSystem.sln في Visual Studio." -ForegroundColor Green
