# Demo 7 — Context Usage Going Red

## Prerequisite

Run this demo **after** Demo 1 (fleet checkout investigation) or the Ask → Plan →
Implement walkthrough in the same session, so there is real accumulated history to
point at. Running `/usage` and `/context` cold, at the start of a session, will not
show anything dramatic.

## Steps to show

1. In the same Copilot CLI session you already used for the checkout investigation, run `/usage`.
2. Read aloud: session duration, models used, token totals by model, edited lines.
3. Run `/context`.
4. Read aloud: system and tool allocation, message-history allocation, free space, reserved buffer.
5. Ask Copilot to recall a specific fact from early in the session, e.g. the exact failing
   test name or the exact discount/total numbers from the first demo.
6. If the answer is vague, imprecise, or re-explores the codebase instead of answering
   directly — stop and point it out to the room. If the answer is still precise, say so
   honestly and move on; do not force the point.

## Prompt (step 5)

```text
What was the exact discount and total our first demo showed before the fix, and which
test file proved it?
```

## Expected outcome

```text
/usage shows session duration, models used, token totals, and edited lines.
/context shows system/tool allocation, message-history allocation, and free space.

The recall prompt either answers precisely (demonstrating healthy context) or answers
vaguely / re-explores files (demonstrating degraded recall) — both outcomes are usable
teaching moments, call out whichever one actually happens.
```

## Say this

```text
"I used this once, wondering why Copilot was taking so long to respond. Context is
working memory — as a session grows, you get accumulating evidence competing with
source files, instructions, prior approaches, plans, and decisions."
```

## Reset

Nothing to reset — these are read-only commands.
