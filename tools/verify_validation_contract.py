#!/usr/bin/env python3
"""Static contract checks for UI input-validation coverage."""
from pathlib import Path
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
ui_dir = ROOT / "UI"
helper = (ROOT / "Helpers" / "UIHelper.cs").read_text(encoding="utf-8")
failures = []

form_files = sorted(ui_dir.glob("*Form.cs"))
covered = []
uncovered = []
field_count = 0
for path in form_files:
    text = path.read_text(encoding="utf-8")
    designer = path.with_name(path.stem + ".Designer.cs")
    designer_text = designer.read_text(encoding="utf-8") if designer.exists() else ""
    field_pattern = r"\b(TextBox|KryptonTextBox|ComboBox|NumericUpDown|DateTimePicker|MaskedTextBox)\b"
    has_input = bool(re.search(field_pattern, designer_text))
    has_validation = bool(re.search(r"ApplyInputValidation|UIHelper\.ApplyStyle|ApplyStyle\(", text))
    if has_input:
        field_count += len(re.findall(field_pattern, designer_text))
        if has_validation:
            covered.append(path.name)
        else:
            uncovered.append(path.name)

checks = {
    "helper_exposes_apply_input_validation": "public static void ApplyInputValidation(Control root)" in helper,
    "helper_handles_textbox": "TextInput_KeyPress" in helper and "TextInput_Validating" in helper,
    "helper_handles_email_and_phone": "IsEmailField" in helper and "IsPhoneField" in helper,
    "helper_handles_money_and_numeric": "IsMoneyField" in helper and "IsIdentityOrNumericField" in helper,
    "helper_normalizes_text": "NormalizeText" in helper and "RemoveEmptyEntries" in helper,
    "helper_limits_birth_date": "ConfigureDateInput" in helper and "MaxDate = DateTime.Today" in helper,
    "all_input_forms_are_covered": not uncovered,
}

for name, passed in checks.items():
    print(f"{'PASS' if passed else 'FAIL'}: {name}")
print(f"INFO: forms={len(form_files)} covered={len(covered)} input_controls_seen={field_count}")
if uncovered:
    print("INFO: uncovered=" + ", ".join(uncovered))

if not all(checks.values()):
    sys.exit(1)
print("PASS: validation coverage contract")
