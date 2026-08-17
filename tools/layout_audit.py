from pathlib import Path
import re

ASSIGN = re.compile(r"this\.(\w+)\.(Location|Size)\s*=\s*new System\.Drawing\.Size?\(([^;]+)\);")
# Separate patterns because Point and Size use different constructors.
LOC = re.compile(r"this\.(\w+)\.Location\s*=\s*new System\.Drawing\.Point\((\d+)\s*,\s*(\d+)\);")
SIZE = re.compile(r"this\.(\w+)\.Size\s*=\s*new System\.Drawing\.Size\((\d+)\s*,\s*(\d+)\);")
PARENT = re.compile(r"this\.(\w+)\.Controls\.Add\(this\.(\w+)\);")
DOCK = re.compile(r"this\.(\w+)\.Dock\s*=\s*System\.Windows\.Forms\.DockStyle\.(\w+);")

for path in sorted(Path('UI').glob('*.Designer.cs')):
    text = path.read_text(errors='ignore')
    loc = {m.group(1):(int(m.group(2)),int(m.group(3))) for m in LOC.finditer(text)}
    size = {m.group(1):(int(m.group(2)),int(m.group(3))) for m in SIZE.finditer(text)}
    parent = {child: container for container, child in PARENT.findall(text)}
    dock = {m.group(1):m.group(2) for m in DOCK.finditer(text)}
    findings=[]
    for child, container in parent.items():
        if child not in loc or child not in size or container not in size:
            continue
        # TabPages intentionally share the same client rectangle inside a TabControl.
        if container.lower().startswith('tab') or child.lower().endswith('tab'):
            continue
        x,y=loc[child]; w,h=size[child]; pw,ph=size[container]
        if dock.get(child) not in {'Fill','Top','Bottom','Left','Right'}:
            if x < 0 or y < 0 or x+w > pw or y+h > ph:
                findings.append(f'{container}: {child} خارج حدود الحاوية child=({x},{y},{w},{h}) parent=({pw},{ph})')
    children_by_parent={}
    for child, container in parent.items():
        if (child in loc and child in size and container in size
                and not container.lower().startswith('tab')
                and not child.lower().endswith('tab')
                and dock.get(child) not in {'Fill','Top','Bottom','Left','Right'}):
            children_by_parent.setdefault(container,[]).append(child)
    for container, children in children_by_parent.items():
        for i,a in enumerate(children):
            ax,ay=loc[a]; aw,ah=size[a]
            for b in children[i+1:]:
                bx,by=loc[b]; bw,bh=size[b]
                if ax < bx+bw and bx < ax+aw and ay < by+bh and by < ay+ah:
                    findings.append(f'{container}: تداخل محتمل بين {a} و {b}')
    if findings:
        print(f'[{path}]')
        for finding in findings:
            print(' -', finding)
print('Layout audit completed.')
