"""Static audit-coverage report for mutating application services."""
from pathlib import Path
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
SERVICES = ROOT / "Services"
WRITE_CALL = re.compile(
    r"\b(?:repository|_repository|userRepository|feePlanRepository)\s*\.\s*"
    r"(?:Add|Create|Insert|Update|Delete|Remove|Save|Assign|Return|Mark|Reset|Restore)"
    r"[A-Za-z0-9_]*\s*\("
)
EXCLUDED = {"AuditLogService.cs"}

rows = []
failures = []
for path in sorted(SERVICES.glob("*.cs")):
    text = path.read_text(encoding="utf-8", errors="ignore")
    writes = len(WRITE_CALL.findall(text))
    if path.name in EXCLUDED or writes == 0:
        continue

    has_permission = any(
        token in text
        for token in ("DemandPermission(", "DemandAny(", "HasPermission(")
    )
    has_audit = "AuditLogService" in text and "auditLogService.Record(" in text
    status = "PASS" if has_permission and has_audit else "FAIL"
    rows.append((path.name, writes, has_permission, has_audit, status))
    if status == "FAIL":
        failures.append(path.name)

print("SERVICE | WRITE CALLS | PERMISSION GUARD | AUDIT RECORD | STATUS")
print("--- | ---: | --- | --- | ---")
for name, writes, permission, audit, status in rows:
    print(
        f"{name} | {writes} | {'yes' if permission else 'no'} | "
        f"{'yes' if audit else 'no'} | {status}"
    )

if failures:
    print("FAIL: services without permission or audit coverage: " + ", ".join(failures), file=sys.stderr)
    sys.exit(1)

print(f"PASS: {len(rows)} mutating services have permission and audit coverage")
