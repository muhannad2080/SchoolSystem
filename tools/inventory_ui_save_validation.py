from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]
for path in sorted((ROOT / "UI").glob("*Form.cs")):
    text = path.read_text(encoding="utf-8")
    handlers = list(re.finditer(r"(?:private|public|protected)\s+(?:async\s+)?void\s+((?:btn\w*(?:Add|Save|Update|Delete|Remove|Assign|Record)\w*)|(?:Save|Add|Update|Create|Delete|Remove|Assign|Record)\w*)\s*\([^)]*\)", text))
    for match in handlers:
        handler_name = match.group(1)
        if "_Click" not in handler_name and handler_name not in ("SaveSettingsSilently", "AddPermission"):
            continue
        start = match.start()
        line = text.count("\n", 0, start) + 1
        body_start = text.find("{", match.end())
        if body_start < 0:
            continue
        body = text[body_start: min(len(text), body_start + 2400)]
        validation = bool(re.search(r"Validate\w*\(|TryParse|IsNullOrWhiteSpace|SelectedIndex|SelectedValue|selected\w*Id|_selected\w*Id|DialogResult|ShowWarning|UIHelper\.IsValid", body))
        print(f"{'PASS' if validation else 'REVIEW'}: {path.name}:{line} {match.group(1)}")
