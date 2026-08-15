from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]
pattern = re.compile(r"\bpublic\s+(?:async\s+)?(?:Task<[^>]+>|bool|int|void|[A-Za-z0-9_<>,.?]+)\s+(\w*(?:Add|Create|Update|Save|Delete|Remove|Assign|Record|Set)\w*)\s*\(")
for path in sorted((ROOT / "Services").glob("*Service.cs")):
    text = path.read_text(encoding="utf-8")
    names = pattern.findall(text)
    if names:
        print(f"{path.name}: {', '.join(names)}")
