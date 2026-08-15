from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]
method_re = re.compile(r"\bpublic\s+(?:async\s+)?(?:Task<[^>]+>|bool|int|void|[A-Za-z0-9_<>,.?]+)\s+(\w*(?:Add|Create|Update|Save|Delete|Remove|Assign|Record|Set)\w*)\s*\([^)]*\)", re.M)
markers = ("EnsureCan", "Validate", "TryParse", "IsNullOrWhiteSpace", "ArgumentException", "InvalidOperationException", "UnauthorizedAccessException", "Require")
for path in sorted((ROOT / "Services").glob("*Service.cs")):
    text = path.read_text(encoding="utf-8")
    for match in method_re.finditer(text):
        start = match.end()
        brace = text.find("{", start)
        if brace < 0:
            continue
        depth = 0
        end = brace
        while end < len(text):
            if text[end] == "{": depth += 1
            elif text[end] == "}":
                depth -= 1
                if depth == 0:
                    break
            end += 1
        body = text[brace:end]
        if not any(marker in body for marker in markers):
            print(f"REVIEW: {path.name}:{text.count(chr(10), 0, match.start()) + 1} {match.group(1)}")
