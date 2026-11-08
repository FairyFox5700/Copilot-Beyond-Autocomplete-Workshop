# Checkout discount investigation checkpoint

## Goal

Determine why checkout returns an $11 discount instead of $10, trace the calculation and tests, and establish whether persistence or migrations participate. Done means source-backed findings, current validation results, and a verified handoff here, with no production source or test changes.

## Repository state

- Branch: `main`
- Commit: `acfbb0f3686c89fdadc3d2cb34fd3ac137aef28c`
- Initial working tree: clean (`git status --short` returned no entries).
- Environment: Windows; projects target .NET 8.
- Status: investigation complete; runtime reproduction confirms the defect. Build passes; focused and full tests fail on the documented discount contract. No fix attempted.

## Confirmed evidence

| ID | Claim | Evidence | Status |
|---|---|---|---|
| E1 | The console passes merchandise 100m, shipping 10m, and rate 0.10m directly to the domain calculator; it only formats the returned breakdown. | `demo\src\Store.Checkout.Demo\Program.cs:3-16` | confirmed |
| E2 | The calculator validates nonnegative money and a rate from zero through one, then incorrectly includes shipping in the discount base. | `demo\src\Store.Checkout\CheckoutCalculator.cs:11-29` | confirmed |
| E3 | Current arithmetic is (100 + 10) * 0.10 = 11 discount; total is 100 + 10 - 11 = 99. Correct merchandise-only arithmetic gives 10 discount and 100 total. | `demo\src\Store.Checkout\CheckoutCalculator.cs:24-35`; inputs in E1 | confirmed |
| E4 | The scoped contract excludes shipping from loyalty discounts and places one authoritative calculation in the domain. Root instructions also keep business rules out of the console. | `.github\instructions\checkout-domain.instructions.md:7-10`; `.github\copilot-instructions.md:3-14` | confirmed |
| E5 | `LoyaltyDiscount_AppliesToMerchandiseButNotShipping` asserts discount 10m and total 100m for the exact demo inputs. Other tests cover zero discount and currency rounding with zero shipping. | `demo\tests\Store.Checkout.Tests\CheckoutCalculatorTests.cs:8-36` | confirmed |
| E6 | Test instructions require deterministic xUnit tests and exact decimal money assertions. No expectations should be weakened. | `.github\instructions\tests.instructions.md:7-11`; `.github\copilot-instructions.md:8` | confirmed |
| E7 | Persistence maps order identity, customer email, total, and creation time; the entity has no discount calculation. The demo references only the domain project, and the domain has no persistence dependency. | `demo\src\Store.Checkout.Persistence\CheckoutDbContext.cs:5-25`; `demo\src\Store.Checkout.Persistence\CheckoutOrder.cs:3-12`; `demo\src\Store.Checkout.Demo\Store.Checkout.Demo.csproj:3-5`; `demo\src\Store.Checkout\Store.Checkout.csproj:1-10` | confirmed |
| E8 | The persistence test checks required properties and the total column type through EF model metadata; it does not invoke the calculator, open a database, or run migrations. | `demo\tests\Store.Checkout.Tests\CheckoutDbContextTests.cs:8-24` | confirmed |
| E9 | The initial migration's Up creates CheckoutOrders; Down drops that table (destructive rollback). Neither computes discounts. Migration instructions prohibit generation/application without an explicit request. | `demo\src\Store.Checkout.Persistence\Migrations\20261029200000_InitialCheckout.cs:9-28`; `.github\instructions\migrations.instructions.md:7-12` | confirmed |
| E10 | Executed focused tests fail at line 14: Expected 10, Actual 11.00. Focused results: 2 passed, 1 failed; full results: 3 passed, 1 failed (including the passing persistence metadata test). | Commands below; `LoyaltyDiscount_AppliesToMerchandiseButNotShipping` | confirmed |
| E11 | Executed console prints merchandise US$100.00, shipping US$10.00, discount -US$11.00, total US$99.00, followed by expected discount $10.00 and total $100.00. | Console command below, exit 0 | confirmed |
| E12 | The solution builds successfully with zero warnings and zero errors. | Build command below, exit 0 | confirmed |
| E13 | Final worktree review shows only this checkpoint modified; production sources and tests have no diff, and whitespace checks pass. | `git status --short`, `git diff --stat`, `git diff --check`, and scoped `git diff --exit-code` below | confirmed |

## Decisions

| ID | Decision | Rationale | Consequence |
|---|---|---|---|
| D1 | Investigation only; edit this checkpoint, not production code, tests, or migration history. | Explicit user scope. | The defect remains intentionally unfixed. |
| D2 | Identify the domain discount base as the root cause, not console formatting or storage. | E1-E9 establish the complete demo path and arithmetic. | A future authorized fix would use merchandiseSubtotal alone, preserving the public API and rounding. |
| D3 | Do not generate/apply migrations or run fix/reset scripts. | No schema change is needed or authorized. | No database mutation. |
| D4 | Replace historical checkpoint command claims with only commands executed in this session. | Previous entries did not establish fresh verification and included a production-mutating fix script. | Old results are not current evidence. |
| D5 | Stop at the reproduced test failures and report the evidence. | Root instructions require stopping on test/build failures; the user prohibits production changes. | No speculative fixes, test edits, or further validation reruns. |

## Hypotheses and rejected paths

| ID | Hypothesis | Evidence for/against | Status |
|---|---|---|---|
| H1 | Currency rounding explains the extra dollar. | 110 * 0.10 is exactly 11 before rounding (E2-E3). | rejected |
| H2 | The console recalculates the discount incorrectly. | It only calls Calculate and prints its properties (E1). | rejected |
| H3 | Persistence or a migration creates the wrong discount. | Neither is in the demo dependency/call path; schema code has no discount arithmetic (E7-E9). | rejected |
| H4 | $11 is the final checkout total. | The source assigns 11 to LoyaltyDiscount and 99 to Total (E3). | rejected |

## Commands executed

| Command | Result | Evidence location |
|---|---|---|
| `git status --short` | Exit 0; no entries initially. | Session tool output |
| `git branch --show-current` | Exit 0; main. | Session tool output |
| `git rev-parse HEAD` | Exit 0; acfbb0f3686c89fdadc3d2cb34fd3ac137aef28c. | Session tool output |
| `dotnet build .\CopilotWorkshop.slnx --no-restore` | Exit 0; build succeeded, 0 warnings, 0 errors. | Session tool output; E12 |
| `dotnet test .\demo\tests\Store.Checkout.Tests\Store.Checkout.Tests.csproj --no-build --no-restore --filter FullyQualifiedName~CheckoutCalculatorTests` | Exit 1; 2 passed, 1 failed, 0 skipped, total 3. Expected 10, Actual 11.00 at CheckoutCalculatorTests.cs:14. | Session tool output; E10 |
| `dotnet test .\CopilotWorkshop.slnx --no-build --no-restore` | Exit 1; 3 passed, 1 failed, 0 skipped, total 4. Same contract failure at line 14. | Session tool output; E10 |
| `dotnet run --project .\demo\src\Store.Checkout.Demo\Store.Checkout.Demo.csproj --no-build --no-restore` | Exit 0; discount -US$11.00, total US$99.00; expected discount $10.00, total $100.00. | Session tool output; E11 |
| `git status --short`; `git --no-pager diff --stat`; `git --no-pager diff --check`; `git --no-pager diff -- .\demo\investigations\checkout-discount-checkpoint.md`; `git --no-pager diff --exit-code -- .\demo\src .\demo\tests` | Only this checkpoint modified; diff reviewed; whitespace check and production/test diff exit 0. Git emitted an LF-to-CRLF advisory for the calculator, not a source diff. | Session tool output; E13 |

Read the checkpoint template, root and scoped instructions, solution, calculator, console, tests, persistence sources, migration, and project manifests with read-only tools. No fix script has been executed.

Build ran first; focused tests, full tests, and console reproduction then ran independently in parallel against those built binaries without rebuilding or restoring.

## Subagent handoffs

None; the calculation and persistence boundary were traced directly.

## Unresolved questions and risks

- No unresolved root-cause question for the supplied demo inputs; the known defect remains unfixed as requested.
- The reported $11 refers to the discount, not the total.
- Existing database contents and deployed systems are outside scope; findings concern this repository's demo path.

## Changed files

- `demo\investigations\checkout-discount-checkpoint.md` only (authorized documentation update).

## Next action

Obtain explicit authorization for a production fix before changing the domain discount base to merchandiseSubtotal alone.
