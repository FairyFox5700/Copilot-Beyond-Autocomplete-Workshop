# Demo 2 — Evidence Checkpoint

## Prompt

```text
Use the evidence-checkpoint skill.

Investigate why checkout returns an $11 discount instead of the expected $10.

Do not change production code.

Create or update:
`demo/investigations/live-checkout-checkpoint.md`

Trace the calculation and inspect:
- applicable repository and path-specific instructions;
- the authoritative domain implementation;
- the failing tests and exact expected behavior;
- the console consumer;
- persistence and migrations.

For every confirmed finding, provide an exact file and line range or a command result.

Record:
- the investigation goal;
- repository state;
- confirmed evidence;
- decisions and rationale;
- hypotheses;
- rejected explanations;
- commands and exact results;
- unresolved risks;
- changed files;
- one next action.

Stop before implementation.
```

## Expected outcome

```text
Copilot creates or updates `demo/investigations/live-checkout-checkpoint.md`.

The checkpoint records:
- the domain includes shipping in the discount base;
- the repository rule says shipping is never discountable;
- the focused test expects discount 10 and total 100 but receives discount 11 and total 99;
- the console consumes the domain result without recalculating it;
- persistence and migrations are not involved;
- rounding and migration hypotheses are rejected;
- no production files were changed;
- the next action is to change only the authoritative discount base and then run the verification ladder.
```
