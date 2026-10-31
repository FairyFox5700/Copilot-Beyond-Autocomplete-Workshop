# Demo 3 — Generate Copilot Instructions

## Prompt

```text
/init
```

## Expected outcome

```text
Copilot analyzes the repository and creates or proposes updates to `.github/copilot-instructions.md`.

The generated instructions summarize:
- the .NET 8 solution and project structure;
- `demo/src/Store.Checkout` as the authoritative pricing domain;
- the console application as a consumer of the domain result;
- the EF Core SQLite persistence project and migration location;
- the xUnit test project;
- build, focused-test, full-test, and console-run commands;
- the intentional red workshop baseline: 3 passing tests and 1 intended failure;
- the corrected result: discount $10 and total $100;
- money, public API, test, and migration conventions;
- applicable path-specific instructions, prompts, agents, and skills;
- verification required before claiming completion.

If `.github/copilot-instructions.md` already exists, Copilot proposes improvements that can be reviewed, accepted, or rejected.

No production source code is changed.
```
