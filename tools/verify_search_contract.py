#!/usr/bin/env python3
"""Static contract for safe WinForms DataView search filters."""
from pathlib import Path
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
UI = ROOT / "UI"
failures = []
row_filter_files = []

for path in sorted(UI.glob("*.cs")):
    text = path.read_text(encoding="utf-8")
    if "RowFilter" not in text:
        continue
    row_filter_files.append(path.name)
    if "EscapeDataViewFilterValue" not in text:
        failures.append(f"{path.name}: RowFilter is used without EscapeDataViewFilterValue")

# Direct DataView filters must not receive raw TextBox/ComboBox text.
for path in sorted(UI.glob("*.cs")):
    text = path.read_text(encoding="utf-8")
    if "RowFilter" not in text:
        continue
    for line_no, line in enumerate(text.splitlines(), 1):
        if "RowFilter" in line and ("Text" in line or "SelectedItem" in line) and "safe" not in line.lower() and "EscapeDataViewFilterValue" not in line:
            failures.append(f"{path.name}:{line_no}: possible raw search value in RowFilter")

if failures:
    for failure in failures:
        print(f"FAIL: {failure}")
    sys.exit(1)

print(f"PASS: {len(row_filter_files)} DataView search forms use escaped filter values")
print("PASS: no direct raw control value was detected in RowFilter assignments")
print("PASS: search validation contract")
