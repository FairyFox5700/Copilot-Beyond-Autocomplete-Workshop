# Demo 2 — Evidence Checkpoint + Live Model Switching

This demo does two things in one pass: it shows the evidence-checkpoint skill
preserving investigation findings, and it shows a **real model switch** — a second,
higher-reasoning model continuing the work using only the checkpoint, with zero
re-exploration. Both halves were verified live against this repository before being
written down below.

> Continues directly from Demo 1 (fleet investigation). Say: "Demo 1's fleet already
> told us shipping is wrongly included in the discount base — instead of losing that
> the moment this chat ends, let's preserve it as evidence." The Part 1 prompt below
> deliberately re-confirms the same finding with a single model (rather than re-running
> the fleet) so the room sees the fact survive the handoff, not just repeat the fleet's
> work.

## Prerequisite

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

Expected final line: `BASELINE VERIFIED: exactly 3 passed, 1 intended failure, 0 skipped.`

## Part 1 — Investigate and checkpoint (fast / low-effort model)

Run from the repository root:

```powershell
copilot -p "Investigate why checkout returns $11 instead of $10. Do not change any production source files. Trace the calculation, inspect the relevant instructions and tests, and determine whether persistence or migrations are involved. Then run the evidence-checkpoint skill, which is allowed to write demo/investigations/checkout-discount-checkpoint.md." -s --model claude-haiku-5.5 --reasoning-effort low
```

## Expected outcome (Part 1)

```text
Copilot identifies that the reported discrepancy is reversed: checkout currently
returns an $11 discount and $99 total, while the business rule and test require a
$10 discount and $100 total.

Copilot confirms:
- demo/src/Store.Checkout/CheckoutCalculator.cs includes shipping in the discount base;
- .github/instructions/checkout-domain.instructions.md says shipping is never discountable;
- demo/tests/Store.Checkout.Tests/CheckoutCalculatorTests.cs expects discount 10 and total 100;
- persistence and migrations are not involved.

Only demo/investigations/checkout-discount-checkpoint.md is changed. No production
source file is modified. Confirm with:

  git status --short

Expected: exactly one line, "M demo/investigations/checkout-discount-checkpoint.md".
```

> Presenter note: if the wording "do not change files" is given without the
> checkpoint-file exception, a fast/low-effort model may interpret it too literally and
> write the checkpoint outside the repository instead of updating the committed file.
> The prompt above was tuned to avoid that — keep the exact wording when re-running.

## Steps to show (Part 1)

1. Run the command above and let the full response render.
2. Run `git status --short` — point out exactly one file changed.
3. Open `demo/investigations/checkout-discount-checkpoint.md` and scroll through:
   confirmed evidence table, decisions table, rejected hypotheses, commands and
   results, next action.
4. Say: "This is now a durable, evidence-backed handoff — not a chat message that
   disappears when context fills up."

## Part 2 — Switch models, continue from the checkpoint only (wow moment)

> Before running this, ask the room: "If I now switch to a completely different,
> higher-reasoning model and tell it not to re-read any source files — only the
> checkpoint — do you think it can still tell me the exact fix?"

Run from the repository root:

```powershell
copilot -p "Read only demo/investigations/checkout-discount-checkpoint.md. Do not re-investigate or re-read the source files yourself. Based solely on that checkpoint's evidence and decisions, state the exact one-line code change you would make and which file it is in. Do not apply the change." -s --model gpt-6.1-sol --reasoning-effort high
```

## Expected outcome (Part 2)

```text
In demo\src\Store.Checkout\CheckoutCalculator.cs:25, replace
`var discountBase = merchandiseSubtotal + shipping;` with:

var discountBase = merchandiseSubtotal;

No change applied.
```

## Hand off to Demo 8

> "We now have the fix confirmed by two different models, from a checkpoint, without
> either one needing to re-read the source. Let's actually apply it — that's Demo 8,
> and since we already know the root cause, we can skip Ask mode and go straight to
> Plan mode."

## Say this

> "Notice the second model never opened `CheckoutCalculator.cs`, never re-ran a test,
> never re-read the instructions — it trusted the checkpoint written by a completely
> different model and got the exact right answer immediately. That's the whole point of
> an evidence checkpoint: it lets you switch models mid-investigation without paying the
> cost of re-exploration, and without losing precision to a vague summary."

## Reset

```powershell
git checkout -- demo/investigations/checkout-discount-checkpoint.md
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```
