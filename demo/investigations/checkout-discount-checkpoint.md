# Checkout discount investigation checkpoint

## Goal

Determine why checkout returns an $11 discount instead of $10, identify the expected result, and establish whether persistence or migrations participate. Done means preserving source-backed findings, verification limits, and one next action here. Do not change any other files or repeat the fleet investigation.

## Repository state

- Branch: `main`
- Commit: `0f31c53`
- Initial working tree for this investigation and checkpoint update: pre-existing modification to `.github\copilot-instructions.md`; leave it untouched.
- Environment: Windows; projects target .NET 8.
- Status: three read-only researchers completed the investigation. Source excerpts and an independent in-memory decimal calculation establish the defect. No application tests, build, console execution, or database operations were run in this investigation. No fix attempted.

## Confirmed evidence

| ID | Claim | Evidence | Status |
|---|---|---|---|
| E1 | The demo calls the domain calculator with merchandise 100m, shipping 10m, and rate 0.10m. | `demo\src\Store.Checkout.Demo\Program.cs:3-7`; calculation researcher's source inspection | confirmed |
| E2 | The calculator sets `discountBase = merchandiseSubtotal + shipping`, rounds the discount to two decimal places using `MidpointRounding.AwayFromZero`, and returns total as merchandise plus shipping minus discount. | `demo\src\Store.Checkout\CheckoutCalculator.cs:24-35`; source excerpt returned by calculation researcher | confirmed |
| E3 | Those inputs produce discount 11 and total 99. A merchandise-only base produces discount 10 and total 100. | E1-E2; independent PowerShell decimal arithmetic, exit 0, output recorded below | confirmed |
| E4 | `LoyaltyDiscount_AppliesToMerchandiseButNotShipping` calls `Calculate(100m, 10m, 0.10m)` and asserts `10m` for `LoyaltyDiscount` and `100m` for `Total`. | `demo\tests\Store.Checkout.Tests\CheckoutCalculatorTests.cs:8-15`; exact method excerpt returned by test researcher | confirmed |
| E5 | That test calls the calculator directly without database use. The test project references both domain and persistence; that reference alone does not put persistence in this test's execution path. | Test method in E4; `demo\tests\Store.Checkout.Tests\Store.Checkout.Tests.csproj:15-24` | confirmed |
| E6 | `CheckoutDbContext` exposes orders and maps stored `Total` as required `decimal(18,2)`; it does not produce the discount. | `demo\src\Store.Checkout.Persistence\CheckoutDbContext.cs:5-8,19-21`; source excerpts returned by database researcher | confirmed |
| E7 | The initial migration creates `CheckoutOrders`, including a `Total` column of `decimal(18,2)`; it defines storage rather than discount arithmetic. | `demo\src\Store.Checkout.Persistence\Migrations\20261029200000_InitialCheckout.cs:7-23`; database researcher's schema inspection and excerpt | confirmed |
| E8 | The demo references only the domain project, and the domain has no persistence project reference. Persistence is not invoked on the inspected demo calculation path. | `demo\src\Store.Checkout.Demo\Store.Checkout.Demo.csproj:3-5`; `demo\src\Store.Checkout\Store.Checkout.csproj:1-10`; demo call in E1 | confirmed |

The arithmetic was independently checked, but the application was not executed. Source evidence was supplied by the three read-only researchers; their scopes were not re-investigated during this documentation update.

## Decisions

| ID | Decision | Rationale | Consequence |
|---|---|---|---|
| D1 | Keep the investigation read-only; subsequently update only this checkpoint under explicit authorization. | User's two requests define separate investigation and documentation scopes. | No production, test, instruction, or migration changes. |
| D2 | Identify the domain discount base as the root cause. | E2-E4 show that including shipping produces the exact mismatch with the test contract. | A future authorized fix would use `merchandiseSubtotal` alone, preserving the public API and rounding. |
| D3 | Do not generate/apply migrations or run fix/reset scripts. | E6-E8 show no schema involvement in this failure. | No database mutation or schema change needed for this defect. |
| D4 | Preserve prior test results as historical, not freshly confirmed. | This investigation ran no tests, build, or console command. | Do not claim a current test failure/pass or successful build from inherited checkpoint text. |
| D5 | Do not rerun the fleet investigation or application validation for this documentation-only update. | Explicit user scope; existing source evidence and arithmetic are sufficient for the checkpoint. | Verification is limited to reviewing the checkpoint diff and whitespace. |

## Hypotheses and rejected paths

| ID | Hypothesis | Evidence for/against | Status |
|---|---|---|---|
| H1 | Currency rounding explains the extra dollar. | 110 * 0.10 is exactly 11 before rounding; the merchandise-only result is exactly 10 (E2-E3). | rejected |
| H2 | Console formatting causes the mismatch. | The test asserts the calculator's numeric result directly; the domain arithmetic already produces the wrong value (E2-E4). | rejected |
| H3 | Persistence or a migration creates the wrong discount. | Neither is in the demo calculation path; the failing test calls the calculator directly (E5-E8). | rejected |
| H4 | $11 is the final checkout total. | The source assigns 11 to LoyaltyDiscount and 99 to Total (E3). | rejected |

## Commands executed

| Command | Result | Evidence location |
|---|---|---|
| `git --no-pager status --short` | Exit 0; only pre-existing `.github\copilot-instructions.md` modified before checkpoint edit. Same result at the end of the read-only investigation. | Current session tool output |
| `git --no-pager branch --show-current` | Exit 0; `main`. | Current session tool output |
| `git --no-pager log -1 --format="%h %s"` | Exit 0; `0f31c53`, presenter-script commit. | Current investigation tool output |
| `git --no-pager rev-parse --short HEAD` | Exit 0; `0f31c53`. | Checkpoint-update tool output |
| PowerShell in-memory `[decimal]` arithmetic using 100, 10, 0.10 and `[decimal]::Round(..., 2, [MidpointRounding]::AwayFromZero)` | Exit 0; `Source arithmetic: discount=11.0; total=99.0`; `Expected arithmetic: discount=10.0; total=100.0`. No files written. | Current investigation tool output; E3 |
| Read-only source searches and views by three researchers | Returned cited calculator, test, dependency, and schema evidence. No files changed. | Researcher handoffs below |
| `git --no-pager diff --check`; `git --no-pager diff --stat`; `git --no-pager status --short`; `git --no-pager diff -- .\demo\investigations\checkout-discount-checkpoint.md` | Exit 0; whitespace check passed and checkpoint diff reviewed. Status shows this checkpoint plus the pre-existing instruction-file modification; no other paths changed. | Checkpoint-update tool output |

### Test results and historical validation

**Current investigation:** focused tests, full solution tests, build, and console reproduction were not run. The defect is source-confirmed and its arithmetic independently verified, not freshly reproduced through the test runner.

The checkpoint inherited the following results from an earlier investigation at commit `acfbb0f3686c89fdadc3d2cb34fd3ac137aef28c`. They are retained for provenance only; original execution logs were not reviewed in this session, so their applicability to current HEAD is **unverified**.

| Historical command | Previously recorded result |
|---|---|
| `dotnet build .\CopilotWorkshop.slnx --no-restore` | Exit 0; 0 warnings, 0 errors. |
| `dotnet test .\demo\tests\Store.Checkout.Tests\Store.Checkout.Tests.csproj --no-build --no-restore --filter FullyQualifiedName~CheckoutCalculatorTests` | Exit 1; 2 passed, 1 failed. Expected 10, actual 11.00 at `CheckoutCalculatorTests.cs:14`. |
| `dotnet test .\CopilotWorkshop.slnx --no-build --no-restore` | Exit 1; 3 passed, 1 failed; same discount assertion. |
| `dotnet run --project .\demo\src\Store.Checkout.Demo\Store.Checkout.Demo.csproj --no-build --no-restore` | Exit 0; discount -US$11.00 and total US$99.00. |

## Subagent handoffs

| Agent question | Conclusion | Verified? | Evidence |
|---|---|---|---|
| Calculation researcher: where is the discount produced? | The domain calculator discounts merchandise plus shipping. No files changed. | Source excerpt reviewed; arithmetic independently checked. | E1-E3 |
| Test researcher: what does the test expect? | The direct calculator test expects discount 10 and total 100. No files changed. | Exact method and project-reference excerpts reviewed; tests not executed. | E4-E5 |
| Database researcher: are persistence or migrations involved? | Storage maps the total but is outside the demo calculation path. No files changed. | Context/migration excerpts and project-reference evidence reviewed; no database access. | E6-E8 |

All three investigation todos are complete. Researchers lacked SQL todo-update capability; the coordinator updated their statuses after reviewing the reports. This documentation update did not dispatch any agents.

## Unresolved questions and risks

- No unresolved root-cause question for the supplied demo inputs; the known defect remains unfixed as requested.
- The reported $11 refers to the discount, not the total.
- Existing database contents and deployed systems are outside scope; findings concern this repository's demo path.
- No fresh test/build results establish current runtime status. Historical results must not be presented as a current run.

## Changed files

- This task changes only `demo\investigations\checkout-discount-checkpoint.md`.
- The pre-existing `.github\copilot-instructions.md` modification is unrelated and remains untouched.
- The preceding fleet investigation changed no files.

## Next action

Obtain explicit authorization to change the domain discount base to `merchandiseSubtotal` alone, preserving the public API and rounding, then validate with focused tests, full solution tests, and the solution build without weakening any test.
