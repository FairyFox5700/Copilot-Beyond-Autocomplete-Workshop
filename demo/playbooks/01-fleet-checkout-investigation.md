# Demo 1 — Fleet Checkout Investigation

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
