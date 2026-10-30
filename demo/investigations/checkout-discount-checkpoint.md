# Checkout discount investigation checkpoint

## Goal

Explain why the loyalty checkout example returns a `$99.00` total instead of `$100.00`, identify the smallest safe fix, and preserve enough evidence for another agent to continue without repeating the investigation.

## Repository state

- Branch: `main`
- Presentation state: intentional failing baseline
- Definition of done: identify the authoritative defect, preserve the public API, and name the verification ladder

## Confirmed evidence

| ID | Claim | Evidence | Status |
|---|---|---|---|
| E1 | The domain calculation includes shipping in the discount base. | `demo/src/Store.Checkout/CheckoutCalculator.cs:24-29` | confirmed |
| E2 | The contract says shipping is never discountable. | `.github/instructions/checkout-domain.instructions.md:7-9` | confirmed |
| E3 | The focused test expects a `$10.00` discount and `$100.00` total. | `demo/tests/Store.Checkout.Tests/CheckoutCalculatorTests.cs:9-16` | confirmed |
| E4 | The console application is only a consumer and displays the contract. | `demo/src/Store.Checkout.Demo/Program.cs:3-16` | confirmed |
| E5 | Persistence defines order storage but does not calculate the checkout total. | `demo/src/Store.Checkout.Persistence/CheckoutDbContext.cs:5-24` | confirmed |

## Decisions

| ID | Decision | Rationale | Consequence |
|---|---|---|---|
| D1 | Fix the discount base in `CheckoutCalculator`. | It is the authoritative domain implementation. | No console or persistence change is required. |
| D2 | Preserve the public `Calculate` signature. | The task does not require an API change. | Existing consumers remain compatible. |
| D3 | Do not edit the test expectation. | The test expresses the documented business contract. | The implementation must move from `$11` to `$10`. |

## Rejected hypotheses

| ID | Hypothesis | Evidence | Status |
|---|---|---|---|
| H1 | Currency rounding causes the `$1.00` difference. | The discount base is `$110`; ten percent is exactly `$11`. | rejected |
| H2 | Shipping should be changed in the console application. | The console delegates to `CheckoutCalculator`; repository instructions place business rules in the domain project. | rejected |
| H3 | A database migration is required. | No persisted schema participates in the calculation. | rejected |

## Commands and results

| Command | Result |
|---|---|
| `demo/scripts/Verify-Baseline.ps1` | 3 passed, 1 intended failure, 0 skipped |
| `demo/scripts/Apply-KnownFix.ps1` | Replaces the baseline calculator with the known-good fixture |
| `demo/scripts/Verify-Fixed.ps1` | Expected corrected state: 4 passed, build green, visible total `$100.00` |

## Suggested subagent split

| Question | Required evidence |
|---|---|
| Where is the authoritative calculation? | Domain files and repository instructions |
| What behavior proves the defect? | Focused tests and visible console result |
| Does persistence require a change? | DbContext, migration, and call-flow evidence |

Subagent conclusions remain unverified until the cited evidence is checked.

## Unresolved questions and risks

- None for the known workshop defect.
- A broader loyalty redesign would require new requirements and a fresh plan.

## Changed files

- None during investigation.

## Next action

Change only the discount base from `merchandiseSubtotal + shipping` to `merchandiseSubtotal`, then run focused tests, the full suite, the solution build, the console application, and `git diff`.
