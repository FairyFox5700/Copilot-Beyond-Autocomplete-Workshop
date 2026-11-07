# Demo 10 — Custom Agent Walkthrough (csharp-conventions-reviewer)

> Replaces an earlier draft built around a checkout-domain-specific agent. This agent is
> generic and reusable across any C# file in any repository that has a
> "C# conventions" section in `copilot-instructions.md` — it is not tied to the checkout
> business domain, which makes it a better example of the general custom-agent pattern.

## Fixtures (already committed)

```text
.github/agents/csharp-conventions-reviewer.agent.md
demo/fixtures/csharp-conventions-review/OrderNotifier.cs   (deliberately bad, for the live catch)
```

## Steps to show

1. Open `.github/agents/csharp-conventions-reviewer.agent.md` directly.
2. Point at the three frontmatter fields in order:
   - `name` — how it's selected via `--agent` or the agent picker;
   - `description` — one line: "read-only reviewer... never edits files";
   - `tools: [read, search]` — deliberately narrower than a typical implementation
     agent (no `edit`, no `execute`) because this agent's entire job is to look, not
     to change anything.
3. Read the body aloud: it checks a file or diff against the "C# conventions" section
   of `.github/copilot-instructions.md` — the exact section added earlier in this
   workshop — and reports a pass/violation checklist, never applies a fix itself.
4. Run the clean-file prompt below — show a real file passing cleanly.
5. Run the bad-fixture prompt below — show it catching several real violations.
6. Run the refusal prompt below — show it explicitly declining to edit even when asked.

## Prompt (clean file — expect a pass)

```powershell
copilot -p "Review demo/src/Store.Checkout.Persistence/CheckoutDbContext.cs." -s --agent csharp-conventions-reviewer
```

## Expected outcome (clean file)

```text
No C# convention violations found in demo/src/Store.Checkout.Persistence/CheckoutDbContext.cs.
- Pass — File-scoped namespace
- Pass — Sealed class and primary constructor
- Pass — Nullable handling
- Pass — Explicit EF configuration
- Pass — Remaining conventions
Read-only review; no files changed or tests/build run.
```

## Prompt (bad fixture — expect real violations)

```powershell
copilot -p "Review demo/fixtures/csharp-conventions-review/OrderNotifier.cs." -s --agent csharp-conventions-reviewer
```

## Expected outcome (bad fixture)

```text
Six convention violations in demo/fixtures/csharp-conventions-review/OrderNotifier.cs:
- File-scoped namespace — violation
- Sealed types — violation
- Cancellation propagation — violation
- Comments (restates code) — violation
- Guard helpers (manual if/throw instead of ArgumentNullException.ThrowIfNull) — violation
- Nullable handling (unnecessary null-forgiving operator) — violation
Plus a correctness note: async void should be Task so callers can await/observe exceptions.
```

## Prompt (refusal check — expect it to decline)

```powershell
copilot -p "Fix the violations you found in demo/fixtures/csharp-conventions-review/OrderNotifier.cs and save the file." -s --agent csharp-conventions-reviewer
```

## Expected outcome (refusal check)

```text
The file remains unchanged. I'm a read-only reviewer; applying and saving these fixes
requires an agent with edit permissions.
```

## Say this

> "This agent only has `read` and `search` tools — no `edit`, no `execute`. That's not a
> suggestion in the description, it's enforced: watch what happens when I explicitly ask
> it to fix and save the file anyway." [run the refusal prompt] "It refuses, every time,
> because the tools list is the actual permission boundary, not just documentation."

## Reset

Nothing to reset — every prompt in this demo is read-only. Confirm with:

```powershell
git status --short
```

Expected: no output (nothing changed).
