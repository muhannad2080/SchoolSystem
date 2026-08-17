import os
import re
import sys

BASE = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
UI = os.path.join(BASE, "UI")

CTRL_RE = re.compile(r'this\.([A-Za-z_\u0600-\u06FF][\w\u0600-\u06FF]*)\.(Location|Size|Dock|Anchor|MinimumSize|MaximumSize|AutoSize|RightToLeft|RightToLeftLayout|Parent|TabIndex|Margin|Padding|Name)\s*=\s*([^;]+);')
CONTROLS_ADD_RE = re.compile(r'this\.([A-Za-z_\u0600-\u06FF][\w\u0600-\u06FF]*)\.Controls\.Add\(this\.([A-Za-z_\u0600-\u06FF][\w\u0600-\u06FF]*)\);')
DOCK_STATIC = 'System.Windows.Forms.DockStyle.None'
POINT_RE = re.compile(r'new System\.Drawing\.Point\((-?\d+),\s*(-?\d+)\)')
SIZE_RE = re.compile(r'new System\.Drawing\.Size\((-?\d+),\s*(-?\d+)\)')
PADDING_RE = re.compile(r'new System\.Windows\.Forms\.Padding\((-?\d+)(?:,\s*(-?\d+))?\)')

def parse_int_hex(v):
    v = v.strip()
    try:
        return int(v)
    except ValueError:
        return 0

def parse_point_from_tuple(tup):
    m = POINT_RE.search(tup)
    if m:
        return (int(m.group(1)), int(m.group(2)))
    return None

def parse_size_from_tuple(tup):
    m = SIZE_RE.search(tup)
    if m:
        return (int(m.group(1)), int(m.group(2)))
    return None

def analyze_file(path):
    with open(path, 'r', encoding='utf-8-sig', errors='replace') as f:
        content = f.read()

    # collect per-control assigned values (last assignment wins)
    ctrl = {}
    for m in CTRL_RE.finditer(content):
        name = m.group(1)
        prop = m.group(2)
        val = m.group(3).strip()
        if name == 'lbl' or name == '':
            continue
        d = ctrl.setdefault(name, {})
        d[prop] = val

    # collect parent relationships
    parents = {}
    order = []
    for m in CONTROLS_ADD_RE.finditer(content):
        parent = m.group(1)
        child = m.group(2)
        parents.setdefault(child, []).append(parent)
        order.append((child, parent))

    # form size
    form_size = None
    for m in re.finditer(r'this\.ClientSize\s*=\s*new System\.Drawing\.Size\((\d+),\s*(\d+)\);', content):
        form_size = (int(m.group(1)), int(m.group(2)))

    def resolved_size(name):
        d = ctrl.get(name, {})
        s = d.get('Size')
        if s and DOCK_STATIC in (d.get('Dock') or '') or (d.get('Dock','').endswith('.None')):
            parsed = parse_size_from_tuple(s)
            if parsed:
                return parsed
        elif s and (d.get('Dock','') == ''):
            parsed = parse_size_from_tuple(s)
            if parsed:
                return parsed
        return None

    def parent_bounds(name):
        # find immediate parent from parents list (last added is actual parent)
        plist = parents.get(name, [])
        if not plist:
            return None
        parent = plist[-1]
        if parent == '':
            return None
        return ctrl.get(parent)

    def static_size(name):
        d = ctrl.get(name, {})
        dock = d.get('Dock', '')
        if 'None' in dock or dock.endswith('Never'):
            s = d.get('Size')
            if s:
                return parse_size_from_tuple(s)
        return None

    def min_size(name):
        d = ctrl.get(name, {})
        s = d.get('MinimumSize')
        if s:
            return parse_size_from_tuple(s)
        return None

    # detect overlaps among non-docked siblings under same parent
    container_children = {}
    for child, parent in order:
        container_children.setdefault(parent, []).append(child)

    problems = []
    for parent, children in container_children.items():
        rects = []
        for child in children:
            d = ctrl.get(child, {})
            dock = d.get('Dock', '')
            if dock and 'None' not in dock:
                continue
            loc = d.get('Location')
            s = d.get('Size')
            if not loc or not s:
                continue
            p = parse_point_from_tuple(loc)
            sz = parse_size_from_tuple(s)
            if not p or not sz:
                continue
            rect = (p[0], p[1], p[0]+sz[0], p[1]+sz[1])
            rects.append((child, rect))
        # صفحات TabControl تتشارك المساحة عمدًا، فلا تُعد تداخلًا.
        parent_info = ctrl.get(parent, {})
        parent_name = parent_info.get('Name', '').strip('"').lower()
        parent_is_tab = parent_name in ('tabcontrol', 'tabs') or parent.lower().startswith('tab')
        # check overlap pairwise (same container)
        for i in range(len(rects)):
            for j in range(i+1, len(rects)):
                a, ra = rects[i]
                b, rb = rects[j]
                if parent_is_tab:
                    continue
                ox = min(ra[2], rb[2]) - max(ra[0], rb[0])
                oy = min(ra[3], rb[3]) - max(ra[1], rb[1])
                if ox > 0 and oy > 0:
                    inter = ox * oy
                    area_a = (ra[2]-ra[0]) * (ra[3]-ra[1])
                    area_b = (rb[2]-rb[0]) * (rb[3]-rb[1])
                    min_area = max(area_a, area_b)
                    if area_a == 0 or area_b == 0:
                        continue
                    ratio = inter / min(max(area_a, area_b), 1)
                    # Ignore true containment of label inside panel (panel likely has extra children - only flag if same level and meaningful)
                    if inter / min(area_a, area_b) > 0.02 and ratio > 0.02:
                        problems.append(f"OVERLAP parent={parent}: {a} {ra} vs {b} {rb} overlap={inter}")

    # detect controls extending beyond parent bounds (static positioned)
    for child, plist in order:
        d = ctrl.get(child, {})
        dock = d.get('Dock', '')
        if dock and 'None' not in dock:
            continue
        loc = d.get('Location')
        s = d.get('Size')
        if not loc or not s:
            continue
        p = parse_point_from_tuple(loc)
        sz = parse_size_from_tuple(s)
        if not p or not sz:
            continue
        parent = plist[-1] if plist else None
        if parent == 'this' or parent is None:
            target_size = form_size or (800, 600)
            if sz[1] > target_size[1] or sz[0] > target_size[0]:
                continue
            if p[0]+sz[0] > target_size[0] + 2 or p[1]+sz[1] > target_size[1] + 2:
                pass
            continue
        pd = ctrl.get(parent, {})
        if not pd:
            parent = plist[-2] if len(plist) >= 2 else None
            if not parent or parent == 'this':
                continue
            pd = ctrl.get(parent, {})
            if not pd:
                continue
        psz = parse_size_from_tuple(pd.get('Size', ''))
        pdock = pd.get('Dock', '')
        if not psz:
            continue
        # parent may be docked fill/top -> approximate
        padding = pd.get('Padding','')
        pad_l = pad_t = pad_r = pad_b = 0
        m = PADDING_RE.search(padding)
        if m:
            if m.group(2):
                pad_l = pad_t = pad_r = pad_b = int(m.group(1))
            else:
                pad_l = pad_r = int(m.group(1))
        if p[0]+sz[0] > (psz[0] - pad_r) + 2:
            problems.append(f"OUT-OF-BOUNDS right: {child} loc={p} size={sz} parent={parent} psize={psz}")
        if p[1]+sz[1] > (psz[1] - pad_b) + 2:
            problems.append(f"OUT-OF-BOUNDS bottom: {child} loc={p} size={sz} parent={parent} psize={psz}")

    return problems, ctrl

def main():
    all_problems = []
    good = 0
    for fn in sorted(os.listdir(UI)):
        if not fn.endswith('.Designer.cs'):
            continue
        path = os.path.join(UI, fn)
        problems, ctrl = analyze_file(path)
        if problems:
            print(f"\n=== {fn} ({len(problems)} issues) ===")
            for p in problems:
                print("   " + p)
            all_problems.extend(problems)
        else:
            good += 1
    # also mainform
    mf = os.path.join(BASE, 'MainForm.Designer.cs')
    if os.path.exists(mf):
        problems, ctrl = analyze_file(mf)
        if problems:
            print(f"\n=== MainForm.Designer.cs ({len(problems)} issues) ===")
            for p in problems:
                print("   " + p)
            all_problems.extend(problems)
        else:
            good += 1
    print(f"\n\nTotal issues: {len(all_problems)}  |  clean files: {good}")

if __name__ == '__main__':
    main()