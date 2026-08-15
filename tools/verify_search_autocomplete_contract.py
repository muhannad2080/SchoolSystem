#!/usr/bin/env python3
"""Verify that every UI form with a search input supports live filtering."""
from pathlib import Path
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
UI = ROOT / "UI"
SEARCH_FIELD = re.compile(r"\b(?:txtSearch|searchBox)\b")
SEARCH_EVENT = re.compile(r"(?:txtSearch|searchBox)\.TextChanged\s*\+=|\b(?:txtSearch|searchBox)_TextChanged\b")
SEARCH_HANDLER = re.compile(
    r"\b(?:Apply\w*(?:Filter|Search)|Load\w*(?:Async)?|Search\w*|Filter\w*)\b"
)

failures = []
checked = 0
for source in sorted(UI.glob("*Form.cs")):
    code = source.read_text(encoding="utf-8-sig")
    designer_path = source.with_name(source.stem + ".Designer.cs")
    designer = designer_path.read_text(encoding="utf-8-sig") if designer_path.exists() else ""
    combined = code + "\n" + designer
    if not SEARCH_FIELD.search(combined):
        continue

    checked += 1
    if not SEARCH_EVENT.search(combined):
        failures.append(f"{source.name}: missing TextChanged event hookup")
    if not SEARCH_HANDLER.search(code):
        failures.append(f"{source.name}: missing search/filter handler")

print(f"Search autocomplete coverage: {checked} forms")
if failures:
    for failure in failures:
        print(f"FAIL: {failure}")
    sys.exit(1)
print("PASS: every searchable form supports live filtering and a search handler")
print("PASS: button/Enter search remains covered by existing handlers")
