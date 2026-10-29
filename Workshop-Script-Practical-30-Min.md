# What GitHub Copilot Can Do Beyond Autocomplete

**Presenter-ready practical workshop**  
**Audience:** Developers who already use GitHub Copilot Chat but mostly ask ad hoc coding questions  
**Duration:** 30 minutes  
**Stack:** VS Code, GitHub Copilot, .NET 8, xUnit, PowerShell  
**Observable outcome:** Participants can turn a vague Copilot request into a repeatable, repository-aware, test-backed workflow using project instructions, path-specific instructions, a prompt file, and a verification loop.  
**Demo scenario:** A loyalty discount incorrectly discounts shipping. For merchandise **$100**, shipping **$10**, and a **10%** discount, the broken result is discount **$11** and total **$99**; the contract requires discount **$10** and total **$100**.

> Presenter note: This is an original Copilot workshop inspired by the source talk’s central pattern—configure durable context, improve daily workflows, automate feedback, and use the assistant beyond code—not a line-by-line conversion of the Claude Code talk.[1]
>
> GitHub Copilot uses repository and path-specific instructions for durable context.[2][3]
>
> Prompt files and custom agents provide separate reusable workflow and specialist surfaces.[4][5]

---

## Presenter preflight — outside the clock

1. Open `Copilot-Beyond-Autocomplete-Workshop` as the VS Code workspace root—not the `demo` subfolder.
2. Sign in to GitHub Copilot.
3. Confirm these files are visible:
   - `.github/copilot-instructions.md`
   - `.github/instructions/checkout-domain.instructions.md`
   - `.github/instructions/tests.instructions.md`
   - `.github/prompts/fix-checkout.prompt.md`
   - `.github/prompts/release-handoff.prompt.md`
   - `.github/agents/checkout-engineer.agent.md`
4. Open the Chat view and select **Agent** mode. If the custom-agent picker is available, confirm `checkout-engineer` appears, but keep the standard Agent selected at the start.
5. In a PowerShell terminal at the repository root, run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

Expected final line:

```text
BASELINE VERIFIED: exactly 2 passed, 1 intended failure, 0 skipped.
```

6. Keep these tabs open:
   - `demo/src/Store.Checkout/CheckoutCalculator.cs`
   - `demo/tests/Store.Checkout.Tests/CheckoutCalculatorTests.cs`
   - `.github/copilot-instructions.md`
   - `.github/instructions/checkout-domain.instructions.md`
   - `.github/prompts/fix-checkout.prompt.md`
7. Start a fresh Copilot chat.
8. Prepare the deterministic fallback:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Apply-KnownFix.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Fixed.ps1
```

9. Do not enable MCP servers or external integrations for this demo. They are unnecessary for the learning outcome and add avoidable latency and permissions.

---

## Run of show

| Time | Segment | Observable result |
|---|---|---|
| 00:00–02:00 | Reproduce the failure | UI output and one exact failing test |
| 02:00–04:00 | Replace the autocomplete mental model | Context → workflow → feedback loop |
| 04:00–07:00 | Show the Copilot context stack | Global and path-specific rules are separated |
| 07:00–10:00 | Try the weak request | Missing scope and verification become visible |
| 10:00–18:00 | Run the engineered Copilot workflow | Minimal domain fix reaches focused green |
| 18:00–22:00 | Verify the result | Focused, full, build, UI, and diff evidence |
| 22:00–26:00 | Participant exercise | Learners design one scoped rule and one reusable prompt |
| 26:00–28:00 | Use Copilot beyond coding | Evidence-backed release handoff |
| 28:00–30:00 | Recap and recovery | Reusable five-step method and reset command |

---

## 00:00–02:00 — Open with a visible failure

### Say

> “Most teams meet GitHub Copilot as autocomplete with a chat box attached. Today I want to show a different model: Copilot as a repository-aware workflow that can investigate, change, verify, and explain its work.”
>
> “We will use one small checkout bug. A loyalty customer buys one hundred dollars of merchandise and pays ten dollars shipping. The loyalty discount is ten percent. Shipping is never discountable, so the discount must be ten dollars and the final total must be one hundred dollars.”
>
> “Let’s see what the application does.”

### Presenter does

Run:

```powershell
dotnet run --project .\demo\src\Store.Checkout.Demo\Store.Checkout.Demo.csproj
```

### Expected

```text
Merchandise: US$100.00
Shipping:    US$10.00
Discount:   -US$11.00
Total:       US$99.00

Contract: shipping is never discountable
Expected discount: $10.00 | Expected total: $100.00
```

Then run:

```powershell
dotnet test .\demo\tests\Store.Checkout.Tests\Store.Checkout.Tests.csproj
```

Point to:

```text
Expected: 10
Actual:   11.00
Failed: 1, Passed: 2, Skipped: 0, Total: 3
```

### Ask

> “If I tell Copilot only ‘fix checkout,’ what information is missing?”

Allow two or three answers: the business rule, authoritative file, allowed scope, and definition of done.

### Transition

> “The model is not the first problem. The shape of the work is the first problem.”

---

## 02:00–04:00 — Teach the minimal mental model

### Say

> “Here is the whole workshop in one sentence: reliable Copilot work needs durable context, a repeatable workflow, and an executable feedback loop.”
>
> “Durable context answers: what is always true in this repository? A repeatable workflow answers: what sequence should we use for this class of task? The feedback loop answers: what evidence proves the result?”
>
> “A long prompt can carry all three once. A maintained repository can carry them for the whole team.”

On a whiteboard or slide, show:

```text
CONTEXT                 WORKFLOW                  FEEDBACK
What is true?     →     What happens next?  →    What proves it?
instructions            prompt / agent           tests / build / diff
```

### Say

> “These are not hard security boundaries. Instructions guide model behavior. Tests, CI, branch protection, permissions, and deployment approvals enforce behavior.”

### Transition

> “Let’s place each kind of information at the narrowest useful scope.”

---

## 04:00–07:00 — Show the Copilot context stack

### Presenter does

Open `.github/copilot-instructions.md`.

### Say

> “GitHub Copilot’s repository-wide instructions live in `.github/copilot-instructions.md`.[2][3] This file should contain rules that are useful for almost every request in this repository.”

Read only these lines aloud:

```text
- Keep checkout business rules in demo/src/Store.Checkout; the console app is only a consumer.
- Use decimal for money and round currency with MidpointRounding.AwayFromZero.
- Never weaken or delete a failing test merely to make the suite pass.
- Before claiming completion, run focused tests, full tests, and the solution build.
```

Then open `.github/instructions/checkout-domain.instructions.md`.

### Say

> “The shipping rule is important, but it does not belong in every conversation. Path-specific instruction files use `applyTo` to attach guidance to matching files.”

Point to:

```yaml
applyTo: "demo/src/Store.Checkout/**/*.cs"
```

Then point to:

```text
- Loyalty discounts apply to merchandise only; shipping is never discountable.
```

Open `.github/instructions/tests.instructions.md` and point to:

```yaml
applyTo: "demo/tests/**/*.cs"
```

### Say

> “This separation matters. Repository-wide rules stay short. Domain rules travel with domain files. Test conventions travel with test files. We avoid one giant instruction document that consumes attention and creates contradictions.”

### Participant action

Ask participants to classify each item verbally:

1. “Use .NET 8.” → repository-wide.
2. “Shipping is never discountable.” → checkout-domain scope.
3. “Use exact decimal assertions.” → test scope.
4. “Fix this one defect.” → current request, not durable instructions.

### Expected answers

```text
Repository-wide, domain-scoped, test-scoped, current request.
```

### Transition

> “Now we will deliberately ignore this structure and see what a weak request gives us.”

---

## 07:00–10:00 — Demonstrate the weak request

### Presenter does

Start a fresh Copilot chat in **Agent** mode. Paste exactly:

```text
Fix the checkout total.
```

If Copilot asks a question, answer only:

```text
The loyalty example is wrong.
```

Let it investigate for no more than 60 seconds. Do not approve broad or unrelated edits.

### Say while it works

> “This request has no example, no explicit invariant, no authoritative target, no scope limit, and no acceptance criteria. Copilot may still succeed because the repository contains useful evidence. But we have made the agent infer the contract and the workflow.”

### Presenter does

Before accepting changes, inspect the proposed plan or diff.

### Ask

> “What would make this response unsafe to accept even if the final number becomes one hundred?”

Listen for:

- it edits the test expectation;
- it duplicates the calculation in the console app;
- it hard-codes this example;
- it changes public APIs unnecessarily;
- it does not run tests;
- it changes unrelated files.

### Say

> “The issue is not that short prompts are always bad. The issue is that one-off intent is not a reusable engineering workflow.”

### Presenter does

Discard any changes from this weak attempt. Run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
```

Start a fresh chat.

### If the weak prompt unexpectedly produces a perfect fix

Say:

> “Good outcome, weak contract. It succeeded this time, but the request still does not make the required checks or allowed scope reviewable. We are improving reproducibility, not trying to make Copilot fail.”

### Transition

> “Now let’s turn the same task into a named, reviewable workflow.”

---

## 10:00–18:00 — Run the engineered Copilot workflow

### Presenter does

Open `.github/prompts/fix-checkout.prompt.md`.

### Say

> “Prompt files are reusable Markdown prompts stored in `.github/prompts`; in VS Code they can define a name, agent, and tools, and can be run from chat.[4] Think of them as team-owned procedures, not magic words.”

Point to the sequence in the file:

```text
1. Read applicable instructions.
2. Run the focused tests.
3. Explain the invariant and identify the authoritative calculation.
4. Propose a minimal plan.
5. Change production code.
6. Run focused tests, full tests, build, and diff.
7. Report evidence.
```

### Say

> “Notice what is not repeated here. The detailed money and domain conventions remain in instruction files. The prompt owns the task sequence.”

### Presenter does

In Copilot Chat, type `/fix-checkout` and run the prompt. If slash discovery does not appear, use **Command Palette → Chat: Run Prompt**, select `fix-checkout`, or paste the body of the file into a fresh Agent chat.

Ask Copilot to stop after planning if your client supports plan-first interaction. Otherwise interrupt before edits and ask:

```text
Before editing, summarize the invariant, authoritative file, failing evidence, and minimal plan in four bullets.
```

### Expected diagnosis

The wording may vary, but it must contain all four facts:

```text
Invariant: loyalty discounts apply to merchandise only; shipping is excluded.
Authoritative production file: demo/src/Store.Checkout/CheckoutCalculator.cs.
Failing evidence: expected discount 10, actual 11.
Minimal change: calculate the discount from merchandiseSubtotal, preserving rounding and the public API.
```

### Say

> “This is the decision checkpoint. The plan is small enough to review before Copilot touches the code.”

### Presenter does

Approve the narrow edit. The expected production change is:

```diff
- var discountBase = merchandiseSubtotal + shipping;
+ var discountBase = merchandiseSubtotal;
```

Copilot may simplify the code to multiply `merchandiseSubtotal` directly. Accept either form if the public API, rounding behavior, and single source of truth remain intact.

### Say

> “The best change here is boring: one business-rule correction in one authoritative file. Agentic does not mean large.”

### Presenter does

Allow Copilot to run the focused test. If it does not, ask:

```text
Run the focused checkout tests now. Do not claim completion yet.
```

### Expected

```text
Passed: 3, Failed: 0, Skipped: 0, Total: 3
```

### Presenter does

Open `.github/agents/checkout-engineer.agent.md` briefly.

### Say

> “For a persistent specialist, Copilot also supports workspace custom-agent files under `.github/agents`.[5] A custom agent can define role and tool access. In this short demo the prompt file is enough; the custom agent shows where we would package the same operating style for repeated checkout work.”

> “Choose the smallest customization that solves the repetition: instructions for durable facts, a prompt for a repeatable request, and a custom agent when a specialist needs a stable role and constrained tool set.”

### If Copilot edits tests or unrelated files

Say:

> “This is exactly why we inspect scope before celebrating green tests.”

Reject those edits and paste:

```text
Restore all test and unrelated-file changes. Fix only the authoritative checkout calculation. Preserve the existing public API and test contract.
```

### If Copilot cannot run

Apply the fallback:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Apply-KnownFix.ps1
```

Then continue with verification. State plainly:

> “The model path diverged, so I am applying the known-good one-line fix. The learning outcome is the workflow and evidence, not a particular model’s wording.”

### Transition

> “A plausible patch is not the finish line. The feedback loop is.”

---

## 18:00–22:00 — Make the feedback loop visible

### Say

> “We verify in widening circles: focused behavior, related behavior, whole solution, visible consumer, then changed-file scope.”

### Presenter does

Run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Fixed.ps1
```

Call out each layer as it appears:

1. Focused checkout tests: 3 of 3 pass.
2. Full solution tests: 3 of 3 pass.
3. Solution build: 0 warnings, 0 errors.
4. Console consumer: discount $10, total $100.

Then run:

```powershell
git diff --stat
git diff -- .\demo\src\Store.Checkout\CheckoutCalculator.cs
```

If the repository has not yet been initialized, use the open editor diff instead and say so.

### Say

> “Tests are executable feedback to both the developer and the agent. The diff is feedback about scope. A green test with a broad, accidental diff is not a clean result.”
>
> “Also notice the control boundary: the instruction says not to edit tests, but our review and CI are what enforce that expectation.”

### Ask

> “Which check caught the customer-visible error? Which check protects us from unrelated edits?”

Expected response:

```text
The domain test and console run catch behavior; the diff review catches scope.
```

### Transition

> “You have seen the pattern. Now you will design one.”

---

## 22:00–26:00 — Participant exercise

### Say

> “You have four minutes. Do not write a giant prompt. Place each requirement in the correct layer.”

### Participant task

Give this scenario:

```text
New rule: free-shipping promotions may set shipping to zero,
but loyalty discounts must still apply only to merchandise.
Every change must include a focused test and must not duplicate pricing logic
in the console application.
```

Ask participants to write three things on the worksheet or in a scratch file:

1. One repository-wide instruction.
2. One checkout-domain instruction.
3. A six-step reusable prompt workflow.

### Say after two minutes

> “Check your scope. If a statement matters only to checkout calculations, it does not belong in the repository-wide file. If it describes this task’s sequence, it belongs in a prompt, not an always-on instruction.”

### Answer key

**Repository-wide:**

```text
- Keep business rules in the domain project; UI and console projects consume domain results and do not recalculate them.
```

**Checkout-domain:**

```text
- A free-shipping promotion may reduce shipping to zero, but loyalty discounts are calculated only from merchandise subtotal.
```

**Reusable workflow:**

```text
1. Read applicable repository and checkout instructions.
2. Add or identify a focused failing boundary test.
3. Run it and capture the exact failure.
4. Change only the authoritative domain calculation.
5. Run focused tests, full tests, build, and the visible consumer.
6. Review the diff and report evidence plus risks.
```

### Debrief

> “The wording can vary. The quality test is whether each statement is durable, correctly scoped, and independently verifiable.”

### Transition

> “Copilot can also turn the same evidence into useful work that is not source code.”

---

## 26:00–28:00 — Use Copilot beyond coding

### Presenter does

Open `.github/prompts/release-handoff.prompt.md`.

### Say

> “The source talk makes an important point: an agent can be an assistant, not only a code generator.[1] Here we reuse the verified repository state to prepare a release handoff.”

Run `/release-handoff` from Copilot Chat, or use **Chat: Run Prompt**.

### Expected shape

```markdown
## Customer impact
Loyalty discounts no longer reduce shipping charges.

## Technical change
The checkout domain calculates the discount from merchandise subtotal only.

## Verification
Focused tests: 3 passed. Full tests: 3 passed. Build succeeded.
Visible example: discount $10.00; total $100.00.

## Risks and rollback
Low-scope domain change; rollback by reverting CheckoutCalculator.cs.
```

### Say

> “The important part is not polished prose. The prompt requires the assistant to inspect the diff and run checks before writing. The handoff is grounded in evidence already produced by the workflow.”

### Transition

> “Let’s close with the reusable method.”

---

## 28:00–30:00 — Recap, recovery, and close

### Say

> “If you remember one sequence, remember this: locate, constrain, plan, verify, package.”

Show:

```text
1. LOCATE the authoritative code and evidence.
2. CONSTRAIN behavior with correctly scoped instructions.
3. PLAN the smallest acceptable change.
4. VERIFY in widening circles: focused → full → build → visible result → diff.
5. PACKAGE repeated work as a prompt or specialist agent.
```

### Say

> “Do not put everything into one instruction file. Do not treat instructions as permissions. Do not accept ‘done’ without executable evidence. And do not build a custom agent when one small prompt file solves the repetition.”
>
> “Autocomplete saves keystrokes. A designed Copilot workflow saves decisions, makes quality visible, and gives the team a process it can review.”

### Final audience question

> “What is one repeated task in your repository that you could package as instructions plus a prompt file next week?”

Take one answer.

### Restore the presentation baseline

Run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

Expected:

```text
BASELINE VERIFIED: exactly 2 passed, 1 intended failure, 0 skipped.
```

### Final line

> “Copilot becomes more useful when your repository teaches it what matters, your workflow tells it what to do next, and your tools prove whether it worked.”

---

## Presenter troubleshooting table

| Problem | Recovery |
|---|---|
| `/fix-checkout` is not listed | Run **Command Palette → Chat: Run Prompt**, choose the file, or paste its body into Agent chat. |
| Custom agent is not visible | Continue with standard Agent mode; the workshop does not depend on custom-agent discovery. |
| Copilot edits tests | Reject those edits and restate the production-only scope. |
| Copilot changes several files | Ask it to restore unrelated files, then apply the known fix if necessary. |
| Model is slow or unavailable | Apply `demo/scripts/Apply-KnownFix.ps1` and continue with the full verification ladder. |
| Tests fail to restore packages | Run `dotnet restore CopilotWorkshop.slnx`; if offline, use the already-restored presenter machine. |
| Currency prefix differs by machine | Verify the numeric discount `10.00` and total `100.00`; locale symbols may differ. |
| Workspace instructions are not discovered | Confirm the repository root—not `demo`—is the opened VS Code workspace. |
| Demo was left green | Run `demo/scripts/Reset-Demo.ps1`, then `Verify-Baseline.ps1`. |

---

## Take-home checklist

- [ ] Keep repository-wide instructions concise and universally relevant.
- [ ] Put specialized rules in path-specific `.instructions.md` files.
- [ ] Put repeatable task sequences in `.prompt.md` files.
- [ ] Use a custom agent only when a stable specialist role or tool boundary is useful.
- [ ] Begin with failing evidence, not a guessed fix.
- [ ] Review the plan before broad edits.
- [ ] Verify focused tests, full tests, build, visible behavior, and diff scope.
- [ ] Treat instructions as guidance; enforce policy with technical controls.
- [ ] Maintain a deterministic fallback and reset path for live demonstrations.

## Sources

[1] https://www.youtube.com/watch?v=P3TCZ2xrvbo — What Claude Code Can Do That You Haven't Tried
[2] https://docs.github.com/en/copilot/customizing-copilot/adding-repository-custom-instructions-for-github-copilot — Adding repository custom instructions for GitHub Copilot
[3] https://code.visualstudio.com/docs/copilot/customization/custom-instructions — Use custom instructions in VS Code
[4] https://code.visualstudio.com/docs/copilot/customization/prompt-files — Use prompt files in VS Code
[5] https://code.visualstudio.com/docs/copilot/customization/custom-agents — Custom agents in VS Code
