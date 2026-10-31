# Demo 2 — Evidence Checkpoint

## Prompt

```text
Investigate why checkout returns $10 instead of $11

Do not change files.

Trace the calculation, inspect the relevant instructions and tests,
and determine whether persistence or migrations are involved.

Let Copilot inspect several files and produce findings.
```

```text
/evidence-checkpoint
```

## Expected outcome

```text
Copilot identifies that the reported discrepancy is reversed: checkout currently returns an $11 discount and $99 total, while the business rule and test require a $10 discount and $100 total.

Copilot confirms:
- `demo/src/Store.Checkout/CheckoutCalculator.cs` includes shipping in the discount base;
- `.github/instructions/checkout-domain.instructions.md` says shipping is never discountable;
- `demo/tests/Store.Checkout.Tests/CheckoutCalculatorTests.cs` expects discount 10 and total 100;
- the console application only displays the domain result;
- persistence and migrations are not involved;
- no migration is required.

After `/evidence-checkpoint`, Copilot updates `demo/investigations/checkout-discount-checkpoint.md` with verified findings, source citations, exact test and console results, repository state, rejected hypotheses, and handoff guidance.

Checkout production code remains unchanged.
```
