"""Static RTL and WinForms designer contract checks for core Arabic screens."""
from pathlib import Path
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
TARGETS = {}
for designer_path in sorted((ROOT / "UI").glob("*.Designer.cs")):
    match = re.search(r"partial class\s+(\w+)", designer_path.read_text(encoding="utf-8", errors="ignore"))
    if not match:
        continue
    name = match.group(1)
    code_path = designer_path.with_name(name + ".cs")
    if code_path.exists():
        TARGETS[name] = (code_path, designer_path)

failures = []
for name, (code_path, designer_path) in TARGETS.items():
    code = code_path.read_text(encoding="utf-8", errors="ignore")
    designer = designer_path.read_text(encoding="utf-8", errors="ignore")
    checks = {
        "partial_class": bool(re.search(r"partial class\s+" + name + r"\b", designer)),
        "single_initialize_component": designer.count("InitializeComponent()") == 1,
        "designer_rtl": "RightToLeft = RightToLeft.Yes" in designer
        or "RightToLeft = System.Windows.Forms.RightToLeft.Yes" in designer,
        "code_rtl": "RightToLeft.Yes" in code or "RightToLeft.Yes" in designer,
    }
    if name == "AuditLogForm":
        checks["status_label_declared"] = "private Label statusLabel;" in designer
    for check_name, passed in checks.items():
        status = "PASS" if passed else "FAIL"
        print(f"{status}: {name}.{check_name}")
        if not passed:
            failures.append(f"{name}.{check_name}")

if failures:
    print("FAIL: RTL/designer checks: " + ", ".join(failures), file=sys.stderr)
    sys.exit(1)

print(f"PASS: {len(TARGETS)} RTL WinForms screens")
