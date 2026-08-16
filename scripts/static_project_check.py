from pathlib import Path
import re
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
errors = []

cs_files = list(ROOT.rglob('*.cs'))
for path in cs_files:
    text = path.read_text(encoding='utf-8-sig', errors='replace')
    if text.count('{') != text.count('}'):
        errors.append(f'{path.relative_to(ROOT)}: unbalanced braces')
    if '\t' in text and path.name.endswith('.Designer.cs'):
        errors.append(f'{path.relative_to(ROOT)}: tabs in Designer file')

for designer in ROOT.rglob('*.Designer.cs'):
    text = designer.read_text(encoding='utf-8-sig', errors='replace')
    code_path = designer.with_name(designer.name.replace('.Designer.cs', '.cs'))
    if not code_path.exists():
        continue
    code = code_path.read_text(encoding='utf-8-sig', errors='replace')
    handlers = set(re.findall(r'\+=\s*new\s+EventHandler\(this\.([A-Za-z_]\w*)\)', text))
    handlers.update(re.findall(r'\+=\s*this\.([A-Za-z_]\w*)', text))
    for handler in sorted(handlers):
        if not re.search(r'\b' + re.escape(handler) + r'\s*\(', code):
            errors.append(f'{designer.relative_to(ROOT)}: missing handler {handler}')

csproj = ROOT / 'SchoolSystem.csproj'
root = ET.parse(csproj).getroot()
ns = {'m': 'http://schemas.microsoft.com/developer/msbuild/2003'}
project_includes = {Path(e.attrib['Include']).as_posix() for e in root.findall('.//m:Compile', ns) if 'Include' in e.attrib}
for include in project_includes:
    normalized = include.replace('\\', '/')
    if not (ROOT / normalized).exists():
        errors.append(f'SchoolSystem.csproj: missing source {include}')

if errors:
    print('\n'.join(errors))
    raise SystemExit(1)

print(f'PASS: checked {len(cs_files)} C# files, Designer handlers, and project includes.')
print('PASS: git diff whitespace check is delegated to git diff --check.')
