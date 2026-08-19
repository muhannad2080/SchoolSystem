#!/usr/bin/env python3
"""Static contract for safe WinForms ComboBox configuration."""
from pathlib import Path
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
UI = ROOT / "UI"

bad = []
combo_files = 0
for path in sorted(UI.glob("*.cs")):
    text = path.read_text(encoding="utf-8", errors="ignore")
    if "ComboBoxStyle.DropDownList" not in text and "DropDownStyle = ComboBoxStyle.DropDownList" not in text:
        continue
    combo_files += 1
    for match in re.finditer(r"AutoComplete(?:Mode|Source)\s*=\s*([^;]+);", text):
        value = match.group(1).strip()
        if not value.endswith("None"):
            line = text.count("\n", 0, match.start()) + 1
            bad.append(f"{path.relative_to(ROOT)}:{line}: incompatible autocomplete value {value}")

if bad:
    print("FAIL: incompatible ComboBox autocomplete configuration")
    print("\n".join(bad))
    sys.exit(1)

print(f"PASS: {combo_files} ComboBox-containing UI files have safe AutoComplete configuration")
