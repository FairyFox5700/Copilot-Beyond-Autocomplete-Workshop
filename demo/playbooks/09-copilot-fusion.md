# Demo 9 — Copilot Fusion (live, ~2 min)

> Upgraded from slide-only to a live demo — verified working in this environment via
> `copilot --model auto`. Fusion visibly prints its own routing decision as the first
> line of output, which makes it a safe, reproducible thing to show on stage.

## Fusion vs. Auto model selection — know the difference before you present

These are two different features, and which one you get depends on your Copilot
surface/plan — they are not both active at once:

- **Auto model selection** picks one model per request based on task complexity and
  system health. It is a model *picker*: one model runs the whole task.
- **Copilot Fusion** is a runtime *router and orchestrator*. For each request it picks
  not just a model but an execution pattern — **Single** (one model), **Cascade** (a fast
  model drafts, a judge escalates to a stronger model if needed), or **Critique** (one
  model drafts, others critique in parallel, the first revises). Different prompts in
  the same session can use different patterns.
- If your Copilot surface shows **Copilot Fusion** in the model picker instead of
  **Auto**, Fusion is what you have — selecting "auto" in that environment routes
  through Fusion, not plain auto-selection. That is exactly what's installed here.

## Prerequisite — update the CLI first

Copilot Fusion is a recent feature; an outdated CLI may not route through it correctly
or may not print the routing banner at all. Update before presenting:

```powershell
copilot update
copilot --version
```

## Steps to show

1. Say the Fusion-vs-Auto distinction above, briefly.
2. Run the simple prompt below — point at the **first line of output**, which names the
   model Fusion picked and why.
3. Run the harder prompt below — point out the complexity rating changes and (depending
   on the day) a different model may be selected.
4. Close with the effort-level note.

## Prompt (simple task)

```powershell
copilot -p "Say OK only." --model auto
```

## Expected outcome (simple task)

```text
● Copilot Fusion selected GPT-6 Luna since the task has low complexity.

OK
```

## Prompt (harder task — reuses the checkout investigation)

```powershell
copilot -p "Investigate why checkout returns $11 instead of $10. Do not change any production source files. Propose the exact one-line fix but do not apply it." --model auto
```

## Expected outcome (harder task)

```text
● Copilot Fusion selected GPT-6 Luna since the task has medium complexity.

[... reads instructions, tests, and the calculator file ...]

Proposed one-line fix, not applied, in demo/src/Store.Checkout/CheckoutCalculator.cs:
var discountBase = merchandiseSubtotal;
```

Note: the exact model named and the complexity label are decided live by Fusion each
run — they may differ from the text above on the day. Do not claim a fixed outcome;
let the room see whatever Fusion actually prints.

## Say this

> "Notice the very first line — Fusion tells you which model it picked and why, before
> doing anything else. On the trivial prompt it picked a lightweight model for low
> complexity. On the investigation prompt, it rated the task higher complexity. This is
> the model-matching instinct from the last demo, done automatically at runtime instead
> of me manually switching `--model` flags myself. You can also set an effort level —
> Fusion Standard, Fusion High, or Fusion Max — to bias how aggressively it escalates to
> stronger models."

## Reset

Nothing to reset — these are read-only prompts (`demo/investigations` and production
source are unchanged; confirm with `git status --short` if you want to show that too).
