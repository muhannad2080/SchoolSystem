[CmdletBinding()]
param(
    [switch]$RestoreLocalChanges
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

$branch = (git branch --show-current).Trim()
if ([string]::IsNullOrWhiteSpace($branch)) {
    throw 'تعذر تحديد فرع Git الحالي.'
}

$dirty = git status --porcelain
$stashCreated = $false
$stashMessage = "school-system-before-main-sync-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

if ($dirty) {
    Write-Host 'تم العثور على تعديلات محلية. سيتم حفظها في stash قبل المزامنة.' -ForegroundColor Yellow
    git stash push --include-untracked --message $stashMessage
    $stashCreated = $true
}

try {
    git fetch origin main
    git switch main
    git merge --ff-only origin/main
    Write-Host 'تم تحديث الفرع main بنجاح من origin/main.' -ForegroundColor Green

    if ($stashCreated -and $RestoreLocalChanges) {
        Write-Host 'محاولة إعادة التعديلات المحلية المحفوظة...' -ForegroundColor Yellow
        git stash pop
        Write-Host 'تمت إعادة التعديلات. إذا ظهر تعارض، عالجه يدوياً ثم نفذ git add وgit commit.' -ForegroundColor Yellow
    }
    elseif ($stashCreated) {
        Write-Host "تم حفظ التعديلات المحلية ولم تُعد تلقائياً. استخدم 'git stash list' ثم 'git stash pop' بعد مراجعة main." -ForegroundColor Cyan
    }
}
catch {
    Write-Error $_
    if ($stashCreated) {
        Write-Host "التعديلات محفوظة في stash باسم قريب من: $stashMessage" -ForegroundColor Yellow
    }
    exit 1
}
