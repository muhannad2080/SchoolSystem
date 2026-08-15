from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]

errors = []
for path in ROOT.rglob('*.cs'):
    text = path.read_text(encoding='utf-8-sig', errors='replace')
    if text.count('{') != text.count('}'):
        errors.append(f'{path}: unbalanced braces')
    if text.count('(') != text.count(')'):
        errors.append(f'{path}: unbalanced parentheses')

main_form = (ROOT / 'MainForm.cs').read_text(encoding='utf-8-sig', errors='replace')
dashboard = (ROOT / 'UI' / 'DashboardHome.cs').read_text(encoding='utf-8-sig', errors='replace')
if 'LoadChart();' in dashboard and not re.search(r'\b(?:private|public|protected|internal)\s+void\s+LoadChart\s*\(', dashboard):
    errors.append('UI/DashboardHome.cs: LoadChart call has no implementation')
if 'LoadAlertsAsync();' in dashboard and not re.search(r'\b(?:private|public|protected|internal)\s+async\s+Task\s+LoadAlertsAsync\s*\(', dashboard):
    errors.append('UI/DashboardHome.cs: LoadAlertsAsync call has no implementation')
if 'MessageBox.Show("خطأ في تحميل الإحصائيات: " + ex.Message)' in dashboard:
    errors.append('UI/DashboardHome.cs: raw exception shown to user')
if 'string.IsNullOrWhiteSpace(ex.Message)' in (ROOT / 'UI' / 'LoginForm.cs').read_text(encoding='utf-8-sig', errors='replace'):
    errors.append('UI/LoginForm.cs: raw exception may be shown to user')

for path in ROOT.rglob('*.cs'):
    text = path.read_text(encoding='utf-8-sig', errors='replace')
    for match in re.finditer(r'new\s+SqlCommand\s*\([^\n]*\+', text):
        errors.append(f'{path}:{text[:match.start()].count(chr(10)) + 1}: SQL command concatenation')

if errors:
    print('\n'.join(errors))
    raise SystemExit(1)

print('Static validation passed: C# delimiter counts, dashboard references, login error handling, and SQL concatenation checks.')
