---
name: release-handoff
description: Produce a non-code handoff from the diff and verification results.
agent: agent
tools: [read, search, execute]
---

Prepare a release handoff for the checkout change.

Use only repository evidence:
- inspect the current diff;
- run the focused checkout tests;
- run the console demo;
- name the business rule affected;
- list verification commands and results;
- state risks or follow-up honestly.

Output exactly these headings:

## Customer impact
## Technical change
## Verification
## Risks and rollback

Do not edit source files. Do not claim a check passed unless you ran it in this session.
