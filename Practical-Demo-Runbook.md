# Practical Demo Runbook

## Start state

Open `Copilot-Beyond-Autocomplete-Workshop` as the workspace root.

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

Expected: exactly 2 passing tests, 1 named failing test, 0 skipped.

## Weak attempt

1. Open a fresh Copilot Agent chat.
2. Paste: `Fix the checkout total.`
3. Let Copilot investigate for at most 60 seconds.
4. Inspect the proposed plan/diff.
5. Reject edits to tests, console calculation, public APIs, or unrelated files.
6. Reset the demo before the engineered run.

## Engineered attempt

1. In Copilot Chat, run `/fix-checkout`.
2. Fallback UI: **Ctrl+Shift+P → Chat: Run Prompt → fix-checkout**.
3. Before editing, require these four facts:
   - shipping is never discountable;
   - authoritative file is `CheckoutCalculator.cs`;
   - expected discount 10, actual 11;
   - smallest fix changes the discount base only.
4. Approve only the production-domain edit.
5. Run the verification script.

## Expected minimal diff

```diff
- var discountBase = merchandiseSubtotal + shipping;
+ var discountBase = merchandiseSubtotal;
```

An equivalent direct multiplication by `merchandiseSubtotal` is acceptable.

## Verification

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Fixed.ps1
```

Expected:

- focused tests: 3 passed;
- full tests: 3 passed;
- build: 0 warnings, 0 errors;
- visible result: discount 10.00, total 100.00.

Review scope:

```powershell
git diff --stat
git diff -- .\demo\src\Store.Checkout\CheckoutCalculator.cs
```

## Deterministic fallback

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Apply-KnownFix.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Fixed.ps1
```

## Reset

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

## Decision branches

| Observation | Action |
|---|---|
| Copilot changes the test expectation | Reject and restate that tests encode the accepted contract. |
| Copilot calculates a corrected value in `Program.cs` | Reject; the console must consume the domain result. |
| Copilot introduces a special case for 100/10/10% | Reject; require a general business-rule fix. |
| Focused tests pass but build fails | Do not proceed; inspect build output and restore unrelated edits. |
| All gates pass but several files changed | Review and reduce scope before completion. |
| Prompt file is not discovered | Run it from Command Palette or paste the prompt body. |
