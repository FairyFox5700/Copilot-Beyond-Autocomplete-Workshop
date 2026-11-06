# Demo 4 — Instruction Hierarchy Conflict (Contradictory Libraries)

## Setup (re-executable fixture, already committed)

```text
demo/fixtures/instruction-hierarchy-audit/
├── AGENTS.md                 # Use xUnit; target .NET 8
├── src/
│   ├── AGENTS.md             # Use NUnit; target .NET 9
│   └── API/
│       └── AGENTS.md         # All endpoints return 200
└── tests/
    └── AGENTS.md             # Use MSTest; target .NET 8
```

No setup steps needed — this folder is deliberately bad and committed as-is so the demo is reusable every time without rebuilding it.

## Steps to show

1. Open the `demo/fixtures/instruction-hierarchy-audit` folder tree in VS Code so all four `AGENTS.md` files are visible at once.
2. Open each file briefly (2–3 seconds each) so the audience reads the contradiction themselves before you say anything.
3. Open Copilot Chat with this folder as context and run the prompt below.
4. Let the response render fully before commenting.
5. Point at the recommended redesign in the response and compare it to the real repository structure (`.github/copilot-instructions.md` + `.github/instructions/*.instructions.md` with `applyTo` globs) — show that your own repo already follows the fixed pattern.
6. Close with the discussion prompt.

## Prompt

```text
Audit this instruction hierarchy.

Identify:
- contradictory rules;
- duplicated guidance;
- unclear scope;
- instructions that belong in deterministic tooling;
- the smallest maintainable replacement structure.

Do not modify files.
```

## Expected outcome

```text
Copilot identifies:
- three incompatible test-framework rules (xUnit / NUnit / MSTest);
- conflicting target-framework versions (.NET 8 vs .NET 9);
- an unsafe HTTP-status rule (all endpoints returning 200);
- no clear precedence or authority between the four files;
- framework choices placed in directories where they do not belong.

Copilot proposes a smaller replacement structure:
- one repository-wide `.github/copilot-instructions.md`;
- path-scoped `.github/instructions/*.instructions.md` files with `applyTo` globs
  for tests and API code specifically.

No files are modified.
```

## Discussion prompt (say this out loud to close the demo)

> "If a rule must never be violated, why is it only written as model guidance instead of being enforced by an analyzer, test, CI rule, permission, or deployment policy?"

## Reset

Nothing to reset — the fixture is static and committed. Re-run any time.
