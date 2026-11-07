# Demo Playbooks — Index

Small, re-executable demo tickets for the "Why Is Copilot Acting Dumb?" 40-minute
session. Each file is self-contained: setup/fixture, step-by-step show instructions,
the exact prompt, expected outcome, and a reset step where relevant.

| # | Demo | File |
|---|---|---|
| 1 | Fleet checkout investigation | `01-fleet-checkout-investigation.md` |
| 2 | Evidence checkpoint | `02-evidence-checkpoint.md` |
| 3 | Generate Copilot instructions (`/init`) | `03-generate-copilot-instructions.md` |
| 4 | Instruction hierarchy conflict (contradictory libraries) | `04-instruction-hierarchy-conflict.md` |
| 5 | Scoped instructions stay out of context (migrations) | `05-scoped-migration-instructions.md` |
| 6 | Skill health check (skill-doctor, live ~1 min) | `06-skill-doctor-mention.md` |
| 7 | Context usage going red | `07-context-usage-audit.md` |
| 8 | Plan mode reveal | `08-plan-mode-reveal.md` |
| 9 | Model / reasoning-effort switching | `09-model-reasoning-handoff.md` |
| 10 | Copilot Fusion (slide only) | `10-copilot-fusion.md` |
| 11 | Custom agent walkthrough (checkout-engineer) | `11-custom-agent-checkout-engineer.md` |
| 12 | Copilot Memory (user preferences and repository facts) | `12-copilot-memory.md` |

## Suggested 40-minute order

3 → 4 → 5 → 11 → 8 → 7 → 9 → 10 → 6 (mention only, anywhere it fits)

## Before presenting

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

Expected final line: `BASELINE VERIFIED: exactly 3 passed, 1 intended failure, 0 skipped.`
