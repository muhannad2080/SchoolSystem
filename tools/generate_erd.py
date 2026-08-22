from __future__ import annotations
import re
from pathlib import Path
from collections import OrderedDict, defaultdict

ROOT = Path(__file__).resolve().parents[1]
SOURCES = [ROOT / 'Databass', ROOT / 'DatabaseBackup']

tables: OrderedDict[str, dict] = OrderedDict()
relations: list[dict] = []

# Keep canonical operational tables and normalized RBAC tables; ignore temp tables.
def norm_table(raw: str) -> str:
    raw = raw.strip().replace('[', '').replace(']', '')
    raw = re.sub(r'^dbo\.', '', raw, flags=re.I)
    return raw.split('.')[-1]

def ensure_table(name: str):
    if not name or name.startswith('#'):
        return None
    if name not in tables:
        tables[name] = {'columns': OrderedDict(), 'pk': [], 'unique': []}
    return tables[name]

def parse_columns(body: str, table: str):
    t = ensure_table(table)
    if not t:
        return
    # Split on commas outside parentheses/quotes.
    parts, start, depth, quote = [], 0, 0, None
    for i, ch in enumerate(body):
        if ch == "'":
            quote = None if quote else "'"
        elif not quote:
            if ch == '(':
                depth += 1
            elif ch == ')':
                depth -= 1
            elif ch == ',' and depth == 0:
                parts.append(body[start:i]); start = i + 1
    parts.append(body[start:])
    for part in parts:
        p = part.strip()
        if not p or re.match(r'^(CONSTRAINT|PRIMARY\s+KEY|FOREIGN\s+KEY|UNIQUE|CHECK|INDEX)\b', p, re.I):
            continue
        m = re.match(r'^\[?([A-Za-z_][\w]*)\]?\s+(.+)$', p, re.S)
        if not m:
            continue
        col, spec = m.group(1), ' '.join(m.group(2).split())
        if col.lower() in {'constraint','primary','foreign','unique','check'}:
            continue
        # Stop at table-level constraint if parser caught it.
        if re.match(r'^(CONSTRAINT|PRIMARY|FOREIGN|UNIQUE|CHECK)\b', spec, re.I):
            continue
        pk = bool(re.search(r'\bPRIMARY\s+KEY\b', spec, re.I))
        t['columns'].setdefault(col, spec)
        if pk and col not in t['pk']:
            t['pk'].append(col)

def add_relation(child, child_cols, parent, parent_cols, constraint=None, delete=None):
    child, parent = norm_table(child), norm_table(parent)
    if not child or not parent:
        return
    ensure_table(child); ensure_table(parent)
    item = {'child': child, 'child_cols': [x.strip(' []') for x in child_cols.split(',')], 'parent': parent,
            'parent_cols': [x.strip(' []') for x in parent_cols.split(',')], 'constraint': constraint or '', 'delete': delete or ''}
    key = (item['child'], tuple(item['child_cols']), item['parent'], tuple(item['parent_cols']))
    if not any((r['child'], tuple(r['child_cols']), r['parent'], tuple(r['parent_cols'])) == key for r in relations):
        relations.append(item)

files = []
for folder in SOURCES:
    files += sorted(folder.glob('*.sql')) + sorted(folder.glob('*.SQL'))
for path in files:
    text = path.read_text(encoding='utf-8', errors='ignore')
    # CREATE TABLE blocks, including optional dbo/schema and IF NOT EXISTS prefix.
    for m in re.finditer(r'CREATE\s+TABLE\s+(?:IF\s+NOT\s+EXISTS\s+)?(?:\[?\w+\]?\.)?\[?([A-Za-z_]\w*)\]?\s*\(', text, re.I):
        table = norm_table(m.group(1)); start = m.end(); depth = 1; i = start; quote = False
        while i < len(text) and depth:
            ch = text[i]
            if ch == "'": quote = not quote
            elif not quote:
                if ch == '(' : depth += 1
                elif ch == ')' : depth -= 1
            i += 1
        parse_columns(text[start:i-1], table)
    # ALTER TABLE ... ADD [column] type (single-column additions).
    for m in re.finditer(r'ALTER\s+TABLE\s+(?:\[?\w+\]?\.)?\[?([A-Za-z_]\w*)\]?\s+ADD\s+(?:\[?([A-Za-z_]\w*)\]?\s+)([^;\n]+)', text, re.I):
        table, col, spec = norm_table(m.group(1)), m.group(2), ' '.join(m.group(3).split())
        t = ensure_table(table)
        if t and col:
            t['columns'].setdefault(col, spec)
    # FOREIGN KEY clauses.
    patterns = [
        r'(?:CONSTRAINT\s+\[?([A-Za-z_]\w*)\]?\s+)?FOREIGN\s+KEY\s*\(([^)]+)\)\s*REFERENCES\s+(?:\[?\w+\]?\.)?\[?([A-Za-z_]\w*)\]?\s*\(([^)]+)\)([^,;\n)]*)',
    ]
    for pat in patterns:
        for m in re.finditer(pat, text, re.I):
            # Find nearest preceding CREATE/ALTER TABLE name in the same statement window.
            before = text[max(0, m.start()-500):m.start()]
            candidates = list(re.finditer(r'(?:CREATE\s+TABLE|ALTER\s+TABLE)\s+(?:IF\s+NOT\s+EXISTS\s+)?(?:\[?\w+\]?\.)?\[?([A-Za-z_]\w*)\]?', before, re.I))
            if not candidates:
                continue
            child = candidates[-1].group(1)
            tail = m.group(5) or ''
            delete = (re.search(r'ON\s+DELETE\s+(CASCADE|SET\s+NULL|NO\s+ACTION)', tail, re.I) or [None, ''])[1]
            add_relation(child, m.group(2), m.group(3), m.group(4), m.group(1), delete)

# Remove clearly temporary/internal tables.
for name in list(tables):
    if name.startswith('#'):
        del tables[name]
relations = [r for r in relations if r['child'] in tables and r['parent'] in tables]

out = ROOT / 'docs' / 'SchoolSystem_ERD.md'
out.parent.mkdir(exist_ok=True)
with out.open('w', encoding='utf-8') as f:
    f.write('# مخطط ERD الكامل لقاعدة بيانات SchoolSystem\n\n')
    f.write('> هذا المستند مولّد من ملفات SQL الموجودة في `Databass/` و`DatabaseBackup/`. عند اختلاف نسخة احتياطية قديمة عن ترحيل أحدث، تُذكر الخصائص المضافة في الترحيلات ضمن الجدول الموحد.\n\n')
    f.write(f'**عدد الجداول المكتشفة:** {len(tables)}  \n**عدد العلاقات المكتشفة:** {len(relations)}\n\n')
    f.write('## مفتاح القراءة\n\n')
    f.write('| الرمز | المعنى |\n|---|---|\n| PK | مفتاح أساسي |\n| FK | مفتاح خارجي |\n| 1:N | واحد إلى متعدد |\n| 1:1 | واحد إلى واحد |\n| N:M | متعدد إلى متعدد عبر جدول وسيط |\n| SET NULL | تبقى السجلات التابعة وتصبح الإشارة فارغة |\n| CASCADE | حذف التابع مع الأصل، ويُستخدم فقط حيث هو معرف في المخطط |\n\n')
    f.write('## الجداول والخصائص\n\n')
    for name, t in tables.items():
        f.write(f'### `{name}`\n\n')
        f.write('| الخاصية | نوع البيانات والقيود | الدور |\n|---|---|---|\n')
        fk_cols = {c for r in relations if r['child']==name for c in r['child_cols']}
        for col, spec in t['columns'].items():
            role = []
            if col in t['pk'] or col.lower() in {x.lower() for x in t['pk']}: role.append('PK')
            if col in fk_cols: role.append('FK')
            f.write(f'| `{col}` | `{spec.replace("|", "\\|")}` | {", ".join(role) or "بيانات"} |\n')
        f.write('\n')
    f.write('## العلاقات والكاردينالية\n\n')
    f.write('| الجدول الابن | الحقل | الجدول الأصل | الحقل الأصل | الكاردينالية | الحذف | القيد |\n|---|---|---|---|---|---|---|\n')
    for r in relations:
        card = '1:N'
        if len(r['child_cols']) > 1: card = '1:N / مركبة'
        f.write(f"| `{r['child']}` | `{', '.join(r['child_cols'])}` | `{r['parent']}` | `{', '.join(r['parent_cols'])}` | {card} | {r['delete'] or 'NO ACTION / غير محدد'} | `{r['constraint'] or 'غير مسمى'}` |\n")
    f.write('\n## العلاقات غير المباشرة N:M\n\n')
    f.write('العلاقة **متعدد إلى متعدد** لا تُخزن عادةً مباشرة؛ تُنفذ بجدول وسيط. في هذا النظام أهم الأمثلة هي `StudentClasses` بين الطلاب والفصول، و`UserRoles` بين المستخدمين والأدوار، و`RolePermissions` بين الأدوار والصلاحيات، و`UserPermissions` بين المستخدمين والصلاحيات. كل صف في الجدول الوسيط يربط سجلاً واحداً من الطرف الأول بسجل واحد من الطرف الثاني، فتتكون علاقة N:M من مجموع علاقتي 1:N.\n\n')
    f.write('## ملاحظات تصميمية مهمة\n\n')
    f.write('1. `AuditLogs.UserID` وحقول `CreatedByUserID` مصممة للحفاظ على السجل التاريخي عند حذف المستخدم، ولذلك يفضل `ON DELETE SET NULL`.\n2. `Vouchers.ReferenceID` حقل مرجعي متعدد الاستخدامات يعتمد على `ReferenceType`، ولا يصح ربطه بمفتاح خارجي واحد إلى جدول محدد.\n3. `Classes.RoomID` علاقة اختيارية؛ الصف قد يُنشأ قبل تخصيص القاعة.\n4. `Enrollments.ClassID` قد يكون اختيارياً في مرحلة القبول الأولى ثم يُملأ عند التوزيع.\n5. يجب تشغيل `Databass/Verify_SchemaIntegrity.sql` على قاعدة `SchoolDB` الفعلية للتحقق من العلاقات والبيانات اليتيمة، لأن ملفات SQL تصف المخطط ولا تعرض حالة البيانات الحالية.\n\n')
    f.write('## مخطط Mermaid\n\n```mermaid\nerDiagram\n')
    for name, t in tables.items():
        f.write(f'    {name} {{\n')
        for col, spec in t['columns'].items():
            dtype = re.sub(r'[^A-Za-z0-9_]', '_', re.match(r'[A-Za-z]+', spec, re.I).group(0) if spec and re.match(r'[A-Za-z]+', spec, re.I) else 'UNKNOWN')
            key = 'PK' if col in t['pk'] else ('FK' if any(r['child']==name and col in r['child_cols'] for r in relations) else '')
            f.write(f'        {dtype} {col} {key} \'{spec[:80].replace("\\'", "") }\'\n')
        f.write('    }\n')
    for r in relations:
        f.write(f'    {r["parent"]} ||--o{{ {r["child"]} : "{", ".join(r["parent_cols"])} -> {", ".join(r["child_cols"])}"\n')
    f.write('```\n')

mermaid = ROOT / 'docs' / 'SchoolSystem_ERD.mmd'
with mermaid.open('w', encoding='utf-8') as f:
    f.write('erDiagram\n')
    for name, t in tables.items():
        f.write(f'    {name} {{\n')
        for col, spec in t['columns'].items():
            dtype = re.sub(r'[^A-Za-z0-9_]', '_', re.match(r'[A-Za-z]+', spec, re.I).group(0) if spec and re.match(r'[A-Za-z]+', spec, re.I) else 'UNKNOWN')
            key = 'PK' if col in t['pk'] else ('FK' if any(r['child']==name and col in r['child_cols'] for r in relations) else '')
            f.write(f'        {dtype} {col} {key}\n')
        f.write('    }\n')
    for r in relations:
        f.write(f'    {r["parent"]} ||--o{{ {r["child"]} : relates\n')

print(f'generated {out}')
print(f'generated {mermaid}')
print(f'tables={len(tables)} relations={len(relations)}')
