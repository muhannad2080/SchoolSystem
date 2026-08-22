import re
from pathlib import Path

root = Path(__file__).resolve().parents[1]
files = list((root / 'Databass').glob('*.sql')) + list((root / 'DatabaseBackup').glob('*.sql'))
tables = {}
foreign_keys = []
for path in files:
    text = path.read_text(encoding='utf-8-sig', errors='ignore')
    for m in re.finditer(r'CREATE\s+TABLE\s+(?:IF\s+NOT\s+EXISTS\s+)?(?:dbo\.)?\[?([A-Za-z0-9_]+)\]?\s*\((.*?)\);', text, re.I | re.S):
        name = m.group(1)
        body = m.group(2)
        tables.setdefault(name, {'files': [], 'columns': set(), 'fks': []})
        tables[name]['files'].append(path.name)
        for col in re.finditer(r'^\s*\[?([A-Za-z0-9_]+)\]?\s+[A-Za-z]', body, re.M):
            tables[name]['columns'].add(col.group(1))
        for fk in re.finditer(r'CONSTRAINT\s+\[?([A-Za-z0-9_]+)\]?\s+FOREIGN\s+KEY\s*\(([^)]+)\)\s+REFERENCES\s+(?:dbo\.)?\[?([A-Za-z0-9_]+)\]?\s*\(([^)]+)\)', body, re.I):
            record = (name, fk.group(2).strip(' []'), fk.group(3), fk.group(4).strip(' []'), path.name)
            tables[name]['fks'].append(record)
            foreign_keys.append(record)
    for fk in re.finditer(r'ALTER\s+TABLE\s+(?:dbo\.)?\[?([A-Za-z0-9_]+)\]?[^;]*?FOREIGN\s+KEY\s*\(([^)]+)\)\s+REFERENCES\s+(?:dbo\.)?\[?([A-Za-z0-9_]+)\]?\s*\(([^)]+)\)', text, re.I | re.S):
        foreign_keys.append((fk.group(1), fk.group(2).strip(' []'), fk.group(3), fk.group(4).strip(' []'), path.name))

print('TABLES')
for name in sorted(tables):
    data = tables[name]
    print(f'{name}\tcolumns={len(data["columns"])}\tfks={len(data["fks"])}\tfiles={";".join(sorted(set(data["files"]))) }')
print('\nFOREIGN_KEYS')
for item in sorted(set(foreign_keys)):
    print('\t'.join(item))
print('\nSUSPECT_ID_COLUMNS_WITHOUT_DECLARED_FK')
for name in sorted(tables):
    cols = sorted(c for c in tables[name]['columns'] if c.endswith('ID') and c not in {name + 'ID', 'ID'})
    fk_cols = {r[1] for r in tables[name]['fks']}
    if cols and not set(cols).issubset(fk_cols):
        print(f'{name}: columns={",".join(cols)}; fk_columns={",".join(sorted(fk_cols))}')

print('\nREPOSITORY_TABLE_REFERENCES')
for path in sorted((root / 'DataAccess').glob('*.cs')):
    text = path.read_text(encoding='utf-8-sig', errors='ignore')
    names = sorted(set(re.findall(r'\b(?:FROM|JOIN|INTO|UPDATE|DELETE\s+FROM)\s+(?:dbo\.)?([A-Za-z][A-Za-z0-9_]*)', text, re.I)))
    if names:
        print(f'{path.name}: {",".join(names)}')
