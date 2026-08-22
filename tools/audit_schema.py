import re
from pathlib import Path

repo_tables = set()
for path in Path('DataAccess').glob('*.cs'):
    text = path.read_text(encoding='utf-8', errors='ignore')
    for pattern in (
        r'\b(?:FROM|JOIN|INTO|UPDATE|DELETE\s+FROM)\s+(?:dbo\.)?\[?([A-Za-z_][A-Za-z0-9_]*)\]?',
    ):
        repo_tables.update(match.group(1) for match in re.finditer(pattern, text, re.I))

sql_tables = set()
for directory in (Path('Databass'), Path('DatabaseBackup')):
    for path in directory.iterdir():
        if path.suffix.lower() != '.sql':
            continue
        text = path.read_text(encoding='utf-8', errors='ignore')
        sql_tables.update(match.group(1) for match in re.finditer(r'\bCREATE\s+TABLE\s+(?:dbo\.)?\[?([A-Za-z_][A-Za-z0-9_]*)\]?', text, re.I))

repo_tables -= {'INFORMATION_SCHEMA', 'STRING_SPLIT', 'sys', 's', 'student_cursor'}
# ReportRepository يحتوي مسار توافق اختياري لا يُنفّذ إلا إذا كانت قاعدة البيانات
# القديمة تستخدم StudentGrades بدلاً من Grades؛ لذلك لا يُعد جدولاً مفقوداً.
legacy_optional_tables = {'StudentGrades'}
missing_tables = (repo_tables - sql_tables) - legacy_optional_tables
print('Repository tables:')
print('\n'.join(sorted(repo_tables)))
print('\nSQL-defined tables:')
print('\n'.join(sorted(sql_tables)))
print('\nReferenced but not defined in project SQL:')
print('\n'.join(sorted(missing_tables)) or '(none)')
print('\nOptional legacy compatibility tables:')
print('\n'.join(sorted(repo_tables & legacy_optional_tables)) or '(none)' )
