# Demo 1 — Fleet Checkout Investigation

> This demo is the first step of a continuous investigation chain, not a standalone
> bug-finding exercise: Demo 1 (fleet, parallel subagents) finds the facts →
> Demo 2 (evidence checkpoint) preserves them and switches models →
> Demo 8 (plan mode) uses the preserved evidence to go straight to a fix, without
> re-investigating. Present them back to back so the audience sees one thread, not
> three unrelated re-investigations of the same bug.

## Prompt

```text
/fleet Investigate the checkout failure.

Use separate read-only researchers for:
1. Finding the calculation that produces the discount.
2. Finding the test and expected result.
3. Checking whether the database or migrations are involved.

Do not change any files.
Return one summary with file paths and line numbers.
```

## Expected outcome

```text
Copilot creates separate read-only research tasks and consolidates their findings.

The final summary identifies:
- `demo/src/Store.Checkout/CheckoutCalculator.cs` as the authoritative calculation;
- shipping is included in the discount base, producing an $11 discount and $99 total;
- `demo/tests/Store.Checkout.Tests/CheckoutCalculatorTests.cs` expects a $10 discount and $100 total;
- the console application only displays the domain result;
- EF Core persistence and migrations do not participate in the calculation;
- no migration is required;
- the smallest prospective correction is to calculate the discount from merchandise only;
- files changed: none.
```

## Say this

> "I'm using separate read-only researchers here because in a large codebase you don't
> want one model trying to hold the whole investigation in its head at once — splitting
> the work by concern (calculation, tests, persistence) and consolidating the findings is
> faster and less error-prone than one long serial exploration."

## Hand off to Demo 2

> "Now instead of losing this summary when the conversation ends, let's preserve it as
> evidence — that's Demo 2."

Carry forward only the conclusion (shipping must not be discounted; fix is in
`CheckoutCalculator.cs`) into Demo 2's investigation prompt — do not re-run a full
fleet investigation there, just let the single model confirm and checkpoint it.

## Reset

Nothing to reset — no files are changed during this demo.
