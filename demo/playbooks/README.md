# Demo Playbooks

## [1 - Generate Copilot instructions](01-generate-copilot-instructions.md)
## [2 - Instruction hierarchy conflict](02-instruction-hierarchy-conflict.md)
## [3 - Scoped migration instructions](03-scoped-migration-instructions.md)
## [4 - Custom agent conventions reviewer](04-custom-agent-conventions-reviewer.md)
## [5 - Fleet checkout investigation](05-fleet-checkout-investigation.md)
## [6 - Evidence checkpoint](06-evidence-checkpoint.md)
## [7 - Plan mode reveal](07-plan-mode-reveal.md)
## [8 - Context usage audit](08-context-usage-audit.md)
## [9 - Copilot Fusion](09-copilot-fusion.md)
## [10 - Skill health check](10-skill-doctor-mention.md)
## [11 - Copilot Memory, separate session](11-copilot-memory.md)

## Reset checkout baseline before Demo 5 and Demo 9

```text
Check git status before resetting checkout.
If resetting would overwrite non-demo work, stop and ask for approval.
Run demo\scripts\Reset-Demo.ps1 and demo\scripts\Verify-Baseline.ps1
from the repository root.
Confirm: BASELINE VERIFIED: exactly 3 passed, 1 intended failure, 0 skipped.
```

## Restore the checkpoint after Demo 7

```text
Restore demo\investigations\checkout-discount-checkpoint.md only if it contains
no pre-existing changes that need to be kept.
If unsure, stop and ask for approval.
```
