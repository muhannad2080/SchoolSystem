#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

fail() {
  echo "FAIL: $1" >&2
  exit 1
}

pass() {
  echo "PASS: $1"
}

grep -q 'Krypton.Toolkit' packages.config || fail 'Krypton Toolkit is not declared in packages.config'
grep -q 'Krypton.Toolkit.dll' SchoolSystem.csproj || fail 'Krypton Toolkit reference is missing from the project'
pass 'Krypton Toolkit integration'

grep -q 'RightToLeft.Yes' Helpers/UIHelper.cs || fail 'RTL configuration is missing from UIHelper'
grep -q 'RightToLeftLayout = true' Helpers/UIHelper.cs || fail 'RTL layout configuration is missing from UIHelper'
pass 'RTL Arabic foundation'

if grep -RInE 'SqlConnection|SqlCommand|SELECT |INSERT |UPDATE |DELETE ' UI --include='*.cs' | grep -vE 'UIHelper|ReportCenterForm.cs:[0-9]+:.*DataView' >/tmp/ui-sql-findings.txt; then
  echo 'SQL statements found in UI files:' >&2
  cat /tmp/ui-sql-findings.txt >&2
  fail 'SQL must remain outside UI layer'
fi
pass 'UI/Repository-Service separation'

service_checks=0
for service in Services/*.cs; do
  [ -f "$service" ] || continue
  if grep -qE 'Permission|HasPermission|UnauthorizedAccessException|CheckPermission' "$service"; then
    service_checks=$((service_checks + 1))
  fi
done
[ "$service_checks" -ge 3 ] || fail "Expected permission checks in at least three services, found $service_checks"
pass 'Service-layer permission checks'

xbuild SchoolSystem.sln /verbosity:minimal >/tmp/schoolsystem-smoke-build.log 2>&1 || {
  cat /tmp/schoolsystem-smoke-build.log >&2
  fail 'xbuild failed'
}
pass 'Clean xbuild compilation'

echo 'All architecture smoke checks passed.'
