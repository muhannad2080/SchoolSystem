from pathlib import Path
import re
import xml.etree.ElementTree as ET

root = Path(__file__).resolve().parents[1]
csproj = root / 'SchoolSystem.csproj'
issues = []

for path in root.rglob('*.cs'):
    if any(part in {'.git', 'bin', 'obj', '.vs'} for part in path.parts):
        continue
    text = path.read_text(encoding='utf-8', errors='ignore')
    if re.search(r'catch\s*\{\s*\}', text):
        issues.append(f'{path}: empty catch block')
    if re.search(r'NotImplementedException|TODO', text, re.IGNORECASE):
        issues.append(f'{path}: TODO or NotImplementedException')
    if re.search(r'\b(?:SELECT|INSERT|UPDATE|DELETE)\b[^;\n]*["\']\s*\+\s*[^=]', text, re.IGNORECASE):
        issues.append(f'{path}: possible SQL string concatenation')

project_text = csproj.read_text(encoding='utf-8', errors='ignore')
for path in root.rglob('*.cs'):
    if any(part in {'.git', 'bin', 'obj', '.vs'} for part in path.parts):
        continue
    relative = path.relative_to(root).as_posix()
    if relative not in project_text.replace('\\', '/'):
        issues.append(f'{path}: C# file not referenced by csproj')

try:
    ET.parse(csproj)
except Exception as exc:
    issues.append(f'{csproj}: invalid XML: {exc}')

if issues:
    print('\n'.join(issues))
    raise SystemExit(1)

print('PASS: full static audit')
