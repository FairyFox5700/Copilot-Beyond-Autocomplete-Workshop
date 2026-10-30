# File upload feature — model-switch handoff

## Goal

Design the smallest safe first slice of a C# API endpoint for authenticated users to upload PDF and PNG files without losing decisions when switching Copilot models midway.

## Current phase

Requirements interview and architecture planning. Production code has not been approved.

## Confirmed decisions

| ID | Decision | Implementation consequence |
|---|---|---|
| D1 | Accept PDF and PNG files up to 10 MB. | Enforce request and streaming limits; add boundary tests. |
| D2 | Do not trust filename extensions or client MIME types. | Inspect content signatures and reject malformed files. |
| D3 | Only authenticated users may upload to their own account. | Enforce identity and ownership at the API boundary. |
| D4 | Detect duplicate content by SHA-256 per owner. | Stream through a hash and return the existing file identifier for duplicates. |
| D5 | Store content privately and metadata in the application database. | Keep object storage and metadata persistence behind separate interfaces. |
| D6 | Files remain quarantined until asynchronous malware scanning succeeds. | Return `202 Accepted`; expose status but not file content while pending. |
| D7 | If metadata persistence fails after upload, attempt object deletion and record cleanup work if deletion fails. | Add compensation behavior and recovery tests. |

## Evidence available

- Presenter decisions: `demo/scenarios/file-upload-interview.md`
- Repository-wide rules: `.github/copilot-instructions.md`
- Requirements-interview workflow: `.github/skills/grill-me/SKILL.md`
- Checkpoint workflow: `.github/skills/evidence-checkpoint/SKILL.md`

## Unresolved questions

- Object-storage provider
- Malware-scanner product, timeout, and retry policy
- Retention and deletion policy
- Upload rate limits
- Polling versus event notification when scanning completes

Do not silently choose these. Propose a provider-neutral interface where possible and ask for approval when the choice affects the first slice.

## Model-switch sequence

### Completed with exploration model

- Repository customization files inspected
- No existing upload implementation found
- Requirements interview captured
- No production files changed

### Next: higher-reasoning model

1. Verify this handoff and cited files.
2. Identify contradictions or unsafe assumptions.
3. Propose the smallest provider-neutral architecture.
4. Produce a test plan and changed-file scope.
5. Do not write code.

### After approval: implementation model

Implement only the approved first slice, run focused and full verification, update this handoff, and review the diff.

## Required verification

- Focused validation and authorization tests
- Duplicate and 10 MB boundary tests
- Storage and compensation failure tests
- Malware quarantine-state tests
- Full solution tests
- Solution build
- Runtime API behavior
- Final diff and changed-file scope

## Changed files

- None. This is still an investigation and planning handoff.
