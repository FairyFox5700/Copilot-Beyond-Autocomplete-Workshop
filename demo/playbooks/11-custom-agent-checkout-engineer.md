# Demo 11 — Custom Agent Walkthrough (checkout-engineer)

## Fixture (already committed)

```text
.github/agents/checkout-engineer.agent.md
```

## Steps to show

1. Open `.github/agents/checkout-engineer.agent.md` directly in VS Code.
2. Point at the three frontmatter fields in order:
   - `name` — how it's selected in the agent picker;
   - `description` — one line, narrow scope, no ambiguity;
   - `tools` — explicitly scoped to `[read, search, edit, execute]`, nothing else.
3. Read the body aloud: identify the business invariant → authoritative production file →
   focused test, before editing; prefer one narrow production change; never alter tests
   to force green; run focused tests, full tests, build, and diff review; report
   evidence, not confidence.
4. Open Copilot Chat and switch the agent picker from standard Agent to `checkout-engineer`.
5. Run the prompt below with the agent selected.

## Prompt

```text
Fix the loyalty discount incorrectly discounting shipping.
```

## Expected outcome

```text
checkout-engineer identifies the business invariant (shipping is never discountable),
the authoritative file (demo/src/Store.Checkout/CheckoutCalculator.cs), and the focused
test that proves the defect, makes one narrow change, then reports focused test results,
full test results, build result, and a diff — not just a claim that it's fixed.
```

## Say this

> "This is a specialist, not a general-purpose assistant — the tools list alone tells
> you what it can and can't do before it writes anything."

## Reset

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```
