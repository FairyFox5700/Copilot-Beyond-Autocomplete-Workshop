# Demo Playbooks — Index

Small, re-executable demo tickets for the "Why Is Copilot Acting Dumb?" 40-minute
session. Each file is self-contained: setup/fixture, step-by-step show instructions,
the exact prompt, expected outcome, and a reset step where relevant.

| # | Demo | File |
|---|---|---|
| 1 | Fleet checkout investigation | `01-fleet-checkout-investigation.md` |
| 2 | Evidence checkpoint + live model switching | `02-evidence-checkpoint.md` |
| 3 | Generate Copilot instructions (`/init`) | `03-generate-copilot-instructions.md` |
| 4 | Instruction hierarchy conflict (contradictory libraries) | `04-instruction-hierarchy-conflict.md` |
| 5 | Scoped instructions stay out of context (migrations) | `05-scoped-migration-instructions.md` |
| 6 | Skill health check (skill-doctor, live ~1 min) | `06-skill-doctor-mention.md` |
| 7 | Context usage going red | `07-context-usage-audit.md` |
| 8 | Plan mode reveal | `08-plan-mode-reveal.md` |
| 9 | Copilot Fusion (live, ~2 min) | `09-copilot-fusion.md` |
| 10 | Custom agent walkthrough (checkout-engineer) | `10-custom-agent-checkout-engineer.md` |
| 12 | Copilot Memory (user preferences and repository facts) | `12-copilot-memory.md` |

## Suggested 40-minute order

3 → 4 → 5 → 10 → 1 → 2 → 8 → 7 → 9 → 6 (live ~1 min, anywhere it fits)

Demos 1, 2, and 8 are a single continuous narrative, not three independent bug
investigations of the same checkout defect — present them back to back:

1. **Demo 1** (fleet) — parallel read-only subagents investigate and find the root cause.
2. **Demo 2** (evidence checkpoint) — a single model confirms and preserves that finding
   as a durable artifact; then a second, higher-reasoning model continues from the
   checkpoint alone, with zero re-exploration (the live model-switching moment).
3. **Demo 8** (plan mode) — skip Ask mode (the root cause is already known) and go
   straight to Plan mode to apply the fix, verify, and close the loop.

Each file's "Say this" / hand-off notes carry the narration between them.

## Before presenting

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

Expected final line: `BASELINE VERIFIED: exactly 3 passed, 1 intended failure, 0 skipped.`
