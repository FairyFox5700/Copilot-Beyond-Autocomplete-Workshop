# What GitHub Copilot Can Do Beyond Autocomplete

A 30-minute, presenter-ready GitHub Copilot workshop with a runnable .NET checkout defect, repository instructions, path-specific instructions, reusable prompt files, a custom-agent example, deterministic red/green verification, participant exercise, and recovery scripts.

## Prerequisites

- VS Code with GitHub Copilot Chat
- .NET 8 SDK
- Windows PowerShell

## Workshop documents

- `Workshop-Script-Practical-30-Min.md` — exact presenter wording, actions, expected states, and timing.
- `Practical-Demo-Runbook.md` — click/run sequence and decision branches.
- `Participant-Worksheet.md` — four-minute exercise and answer key.
- `Slides-Grill-Me-Module.md` — slide-ready material for requirements interviewing, prompt-versus-instructions-versus-skill, approval, and verification.
- `demo/scenarios/file-upload-interview.md` — presenter answer key for the deliberately vague file-upload feature.

## Demo structure

```text
.github/
  copilot-instructions.md
  instructions/
  prompts/
  agents/
  skills/grill-me/SKILL.md
demo/
  src/Store.Checkout/
  src/Store.Checkout.Persistence/
  src/Store.Checkout.Demo/
  tests/Store.Checkout.Tests/
  fixtures/
  scripts/
```

## Verify intentional baseline

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

Expected: 3 passed, 1 intended failure, 0 skipped.

## Apply and verify known fix

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Apply-KnownFix.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Fixed.ps1
```

Expected: focused tests green, full tests green, build green, and visible discount/total correct.

## Restore for presentation

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

The repository is intentionally delivered in the red baseline state. The corrected state and full recovery sequence were verified before packaging.
