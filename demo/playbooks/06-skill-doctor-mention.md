# Demo 6 — Skill Health Check (skill-doctor, live, ~1 min)

> Correction from earlier drafts: skill-doctor diagnoses **skill packages**
> (`SKILL.md` + bundled resources) — metadata, broken links, trigger-description
> quality, and eval hygiene. It does NOT check for contradictory instructions across
> `AGENTS.md`/`copilot-instructions.md` files — that is Demo 4's job. Keep these two
> demos clearly separate when presenting; do not imply skill-doctor replaces the
> instruction-hierarchy audit.

## Prerequisite

Requires `npx` (ships with Node.js). No install step — `npx -y` fetches and runs it
on demand. Confirm once before presenting:

```powershell
npx -y skill-doctor@latest .github/skills
```

Expected: `score: 100 / 100 Excellent`, `evidence-checkpoint` and `grill-me` both `clean`.

## Steps to show

1. Say the line below before running anything.
2. Run the command live in a terminal at the repo root.
3. Let the score table render — point at the two real skills in this repo scoring 100.
4. Say the one-line distinction from skill-hierarchy conflicts (Demo 4).

## Command

```powershell
npx -y skill-doctor@latest .github/skills
```

## Expected outcome

```text
score:    100 / 100 Excellent
coverage: 2 skills • 2 healthy
findings: 0 errors • 0 warnings

evidence-checkpoint  100   clean
grill-me             100   clean
```

## Say this

> "There's a tool called skill-doctor that checks whether your skills are well-formed —
> valid metadata, no broken file references, and a description clear enough that Copilot
> actually knows when to trigger it. It's a different check from the instruction-conflict
> demo I showed earlier — this one is about skill quality, not contradictory rules."

## Reset

Nothing to reset — read-only scan, no files are changed.
