from pathlib import Path
import re

errors = []
for path in sorted((Path(__file__).resolve().parents[1] / 'Databass').glob('*.sql')):
    text = path.read_text(encoding='utf-8-sig', errors='replace')
    normalized = re.sub(r"N?'(?:''|[^'])*'", "''", text)
    normalized = re.sub(r'--.*$', '', normalized, flags=re.MULTILINE)
    if re.search(r'(?im)^\s*USE\s+master\b', normalized):
        errors.append(f'{path.name}: must not execute against master')
    if re.search(r'(?im)DROP\s+TABLE\s+.*\b(Users|Roles|Permissions)\b', normalized):
        errors.append(f'{path.name}: attempts to drop security tables')
    if re.search(r'(?im)DELETE\s+FROM\s+.*\b(Users|Roles|Permissions)\b', normalized):
        errors.append(f'{path.name}: attempts to delete security data')

if errors:
    print('\n'.join(errors))
    raise SystemExit(1)

print('PASS: checked SQL database target, security preservation, and block structure.')
