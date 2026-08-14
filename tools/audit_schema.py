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
for path in Path('Databass').glob('*.sql'):
    text = path.read_text(encoding='utf-8', errors='ignore')
    sql_tables.update(match.group(1) for match in re.finditer(r'\bCREATE\s+TABLE\s+(?:dbo\.)?\[?([A-Za-z_][A-Za-z0-9_]*)\]?', text, re.I))

repo_tables -= {'INFORMATION_SCHEMA'}
print('Repository tables:')
print('\n'.join(sorted(repo_tables)))
print('\nSQL-defined tables:')
print('\n'.join(sorted(sql_tables)))
print('\nReferenced but not defined in project SQL:')
print('\n'.join(sorted(repo_tables - sql_tables)))
