# Demo 8 — Plan Mode Reveal

## Prerequisite

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

Expected final line: `BASELINE VERIFIED: exactly 3 passed, 1 intended failure, 0 skipped.`

## Steps to show

1. Open `demo/src/Store.Checkout/CheckoutCalculator.cs` and `demo/tests/Store.Checkout.Tests/CheckoutCalculatorTests.cs` in tabs.
2. Start in Ask mode. Run the Ask prompt below and let the answer render.
3. Switch to Plan mode. Run the Plan prompt below.
4. **Before scrolling the generated plan**, ask the room: "What do you think it'll propose touching — just the calculator, or will it try to touch the tests too?"
5. Scroll the real generated plan live. Point at the file-change scope it lists.
6. Confirm out loud whether it scoped to the one narrow production file (matching the `checkout-engineer` agent's own rule: "prefer one narrow production change, never alter tests merely to obtain a green result") or whether it proposed touching the test file too.
7. Only after this discussion, move to implementation and verify.

## Prompt (Ask mode)

```text
Explain how the discount calculation works in CheckoutCalculator.cs.
```

## Prompt (Plan mode)

```text
Fix the loyalty discount incorrectly discounting shipping. Merchandise $100, shipping
$10, 10% discount. Expected discount $10 and total $100.
```

## Verify after implementation

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Fixed.ps1
```

## Expected outcome

```text
Ask mode explains that CheckoutCalculator.cs currently includes shipping in the
discount base, producing $11 discount / $99 total instead of the contracted $10 / $100.

The generated plan scopes to demo/src/Store.Checkout/CheckoutCalculator.cs only —
no test file changes proposed, consistent with the checkout-domain instructions
and the checkout-engineer agent's own rule against weakening tests to get a pass.
```

## Reset

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```
