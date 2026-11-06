# Demo 9 — Model / Reasoning-Effort Switching

## Fixture (already committed)

```text
demo/investigations/file-upload-model-handoff.md
```

## Steps to show

1. Open `demo/investigations/file-upload-model-handoff.md` directly — do not run a live
   task for this one, the file already contains the worked example.
2. Scroll to the "Model-switch sequence" section — point at the three stages
   (exploration model → higher-reasoning model → implementation model) and what each
   is and is not allowed to do (e.g. "do not write code" during the reasoning stage).
3. Scroll to the "Reasoning-effort plan" table. Read the phases and suggested effort
   levels aloud.
4. Read the closing line: "Reset effort after the difficult phase rather than leaving
   the highest level enabled for routine work."

## Table to project

| Phase | Suggested effort | Reason |
|---|---|---|
| Repository exploration | Default | Primarily retrieval, classification, and evidence collection |
| Security and failure analysis | Higher | Tenant isolation, quarantine, compensation, and asynchronous states interact |
| Approved implementation | Default or moderate | The contract and scope should already be explicit |
| Targeted security review | Higher | Search for subtle authorization, race, and recovery defects |
| Tests and build execution | Default | Results are deterministic and do not need extended reasoning |

## Say this

> "This file already proves the point I'm making — match the model to the phase, not
> to habit. Architecture and security work gets the high-reasoning model. Implementation
> and test runs don't need it. And critically: reset the effort level afterward, don't
> leave the expensive model running for routine work."

## Reset

Nothing to reset — this is a read-only walkthrough of a committed file.
