# Workshop Script — "Why Is Copilot Acting Dumb?" (40-Minute, Full Notes)

Full presenter notes with complete narration for every section. Each demo links to its
playbook file in `demo/playbooks/` — those files hold the exact prompts; this script
holds what to say, when to run them, and why they're sequenced this way.

All demos after the opening run inside **one continuous interactive `copilot` session**
unless a reset is explicitly called out. Keep that session open throughout — several
demos depend on earlier ones (the fleet investigation's finding feeds the checkpoint,
the checkpoint feeds the model switch, the model switch feeds the plan).

---

## 0:00–1:30 — Open (no demo)

Say, in full:

> "So today's topic of conversation would be why GitHub is sometimes acting dumb, or am
> I dumb, or do I speak another language. The reason behind that is most probably that
> GitHub Copilot, or any other tool you use — Claude or something else, whatever you
> like — is just a tool with a harness that enables you to do most things you used to
> do, in a more advanced way. But the thing is, it still cannot read your thoughts, and
> it probably doesn't have as much context as you do.
>
> To make life easier, there are already several nuances you need to consider for
> yourself when starting your project. I'll try to make it practical, so whatever I
> explain, I'll explain with an example — and I'll use my own workshop repository for
> every single one of these, so you can go clone it afterward and re-run exactly what
> you see today."

Presenter action: have the terminal and VS Code already open, repository cloned, and
the interactive `copilot` session **not yet started** — start it live at the beginning
of Demo 1 so the room sees the session come up from nothing.

---

## 1:30–4:30 — Demo 1: Generate Copilot Instructions

File: `demo/playbooks/01-generate-copilot-instructions.md`

Say:

> "The first thing we're going to do is start with Copilot instructions. What are
> Copilot instructions? The simple way to understand this: if you want your agent to
> remember something every time, put it there. Let's say you have guidance on the
> architecture you use inside your project. You have guidance that you use spec-driven
> development. You have guidance that you use test-driven development. You have guidance about safety rules.
> You have guidance that you use a specific library for validation, but not another one.
>  That's exactly what this is for.
>
> And there's a useful command that can generate this for you in the repository.
> Usually you need a command, and if you look at what's inside this init command, it
> will inspect the project and write Copilot instructions for you. Whether they're the
> best or not, decide for yourself."

Demo steps:
1. Start the interactive session: `copilot`
2. Run:
   ```text
   /init
   ```
3. Let the full output render — Copilot inspects the repo and proposes or updates
   `.github/copilot-instructions.md`.
4. Open the real, committed `.github/copilot-instructions.md` and read two lines aloud —
   for example the money-rounding rule (`Use decimal for money and round currency with
   MidpointRounding.AwayFromZero`) and one line from the "C# conventions" section
   (e.g. nullable handling or sealed-by-default).

Say:

> "There's some confusion among different agents about these Copilot instructions. It's
> generally so useful and so simple to follow that a specific format has been developed
> that works for any tool you use: AGENTS.md. It behaves the same way your Copilot
> instructions do. Don't mix both. Decide what works better for your team. If you only
> use Copilot, use the Copilot instructions. If your team uses different tools, AGENTS.md
> will work in Claude Code and with other providers — the behavior is just the same."

---

## 4:30–11:00 — Demo 2: Instruction Hierarchy Conflict (wow #1)

File: `demo/playbooks/02-instruction-hierarchy-conflict.md`
Fixture: `demo/fixtures/instruction-hierarchy-audit/`

Say:

> "Now, another thing worth mentioning: if, in the repository, you have multiple of
> these agent, cloud, or Copilot instruction files, what's going to happen? Let's say in
> one file you have 'always, always, always use a fluent validator library for
> validation,' and in another file, 'always use a different validation library.' What's
> going to happen? Who knows. Sometimes one instruction will apply, sometimes the other,
> and Copilot will get confused. You'll be lucky if it even asks you a question.
>
> Let me actually show you this, because I've built exactly this situation into a real
> repository, so you can see it live instead of me just describing it."

Demo steps:
1. Open the `demo/fixtures/instruction-hierarchy-audit/` folder tree in VS Code live:
   ```text
   AGENTS.md              → Use xUnit; target .NET 8
   src/AGENTS.md          → Use NUnit; target .NET 9
   src/API/AGENTS.md      → All endpoints return 200
   tests/AGENTS.md        → Use MSTest; target .NET 8
   ```
2. Open each file for 2–3 seconds so the room reads the contradiction themselves before
   you comment.
3. In the interactive session, run:
   ```text
   Audit the AGENTS.md hierarchy under demo\fixtures\instruction-hierarchy-audit.
   Identify contradictory rules, duplicated guidance, unclear scope, and rules
   that belong in deterministic tooling.
   Propose the smallest maintainable replacement structure.
   Do not modify files.
   ```
4. Let the findings render fully — three incompatible test frameworks, two conflicting
   .NET versions, an unsafe "always return 200" rule, no precedence between files.

Say:

> "In general, keep maintaining your instructions as the source of truth. The more
> confusing information they contain, the harder it's going to be for you. And if you
> only see a Copilot instruction file in your repository, also verify whether there's an
> organization-level or repository-level one — there can be a level of instructions in a
> different monorepo, instructions at the organization level, at a path-specific level,
> or in your user profile. If any of these overlap but contain mismatched or conflicting
> information, your Copilot will get confused. And again — it's not the source of truth.
> Sometimes it follows it, sometimes it doesn't. If you write 'never write a comment,'
> there can still be a situation where it writes a comment anyway. There are different
> mechanisms to handle that — probably hooks or behaviors you define afterward — but
> treat the instructions simply as instructions, like you'd treat a human: we sometimes
> follow them, sometimes we don't."

Close with the discussion prompt baked into the fixture (say it, don't just show it):

> "If a rule must never be violated, why is it only written as model guidance instead of
> being enforced by an analyzer, test, CI rule, permission, or deployment policy?"

---

## 11:00–15:00 — Demo 3: Scoped Migration Instructions

File: `demo/playbooks/03-scoped-migration-instructions.md`
Fixture: `.github/instructions/migrations.instructions.md`
(`applyTo: demo/src/Store.Checkout.Persistence/Migrations/**/*.cs`)

Say:

> "Now, what's going to happen if I place a whole PDF file into the Copilot
> instructions? First of all, my context window in the agent is limited. If I fill it
> right now with everything from my Copilot instructions file, how much useful
> information can I actually get from that when I keep asking the agent to explore my
> codebase to find a bug? The context will be overloaded, and there's a huge
> possibility that the attention span will be very limited and information will go
> missing. Ideally, keep global instructions small.
>
> We also know that what we can maintain in memory is limited, right? If we're at the
> point where we have 500+ lines in the compiled instructions, that's maybe a point to
> consider another approach — maybe we're putting too much out-of-context information
> at once for things that shouldn't always be followed. For example, let's say we have a
> specific constraint for our migration files — the data structure, the naming format,
> the naming convention for table IDs. Do I need that every time I generate something?
> Should I read this every single time I'm doing something with Copilot? Probably not."

Demo steps (all in the same interactive session):
1. Run:
   ```text
   /instructions
   ```
   Point out the full list of discovered instruction sources — this is everything that
   *exists*, not necessarily everything that's *active* for a given file.
2. Run:
   ```text
   Which repository instruction files apply to
   demo\src\Store.Checkout\CheckoutCalculator.cs?
   List only applicable paths. Do not modify files.
   ```
   Confirm the migration rules are **absent** from the result.
3. Run:
   ```text
   Which repository instruction files apply to
   demo\src\Store.Checkout.Persistence\Migrations\20261029200000_InitialCheckout.cs?
   List only applicable paths. Do not modify files.
   ```
   Confirm the migration rules now **appear**.


---

## 15:00–19:00 — Demo 4: Custom Agent — Conventions Reviewer

File: `demo/playbooks/04-custom-agent-conventions-reviewer.md`
Agent: `.github/agents/csharp-conventions-reviewer.agent.md`
Fixture: `demo/fixtures/csharp-conventions-review/OrderNotifier.cs`

Say:

> "Now let's look at a specialist, rather than a general-purpose assistant — because
> sometimes you don't want Copilot to behave like a generalist, you want it to behave
> like one specific role on your team."

Demo steps:
1. Open `.github/agents/csharp-conventions-reviewer.agent.md`. Point at each frontmatter
   field in order:
   - `name` — how it's selected.
   - `description` — one line, narrow scope: "read-only reviewer... never edits files."
   - `tools: [read, search]` — deliberately narrower than a typical implementation
     agent. No `edit`, no `execute`. This agent's entire job is to look, not to change
     anything.
2. Select the `csharp-conventions-reviewer` agent in the session.
3. Run (clean file, expect a pass):
   ```text
   Review demo\src\Store.Checkout.Persistence\CheckoutDbContext.cs.
   ```
4. Run (deliberately bad fixture, expect real violations):
   ```text
   Review demo\fixtures\csharp-conventions-review\OrderNotifier.cs.
   ```
   Expect it to catch: block-scoped namespace, non-sealed class, missing
   `CancellationToken` propagation, a comment that restates the code, a manual
   null-check instead of a guard-clause helper, and a stray null-forgiving `!` operator —
   plus a bonus correctness note that `async void` should be `Task`.
5. Run the refusal check (expect it to decline):
   ```text
   Fix the violations you found in
   demo\fixtures\csharp-conventions-review\OrderNotifier.cs and save the file.
   ```

Say:

> "This is a specialist, not a general-purpose assistant — the tools list alone tells
> you what it can and can't do before it writes a single line. Watch what happens when I
> explicitly ask it to fix and save the file anyway." [run the refusal prompt] "It
> refuses, every time, because the tools list is the actual permission boundary, not
> just documentation."

---

### Presenter checkpoint (not spoken) — before Demo 5

Check `git status --short`. If the working tree is clean, run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

Confirm the final line: `BASELINE VERIFIED: exactly 3 passed, 1 intended failure, 0 skipped.`

If resetting would overwrite work you want to keep, stop and check before running the
scripts.

---

## 19:00–22:00 — Demo 5: Fleet Checkout Investigation

File: `demo/playbooks/05-fleet-checkout-investigation.md`

Say:


> Here's a variant of that worth showing: instead of one model doing a long serial
> exploration, you can split the investigation across separate read-only researchers and
> have them converge on one answer."

Demo steps:
1. Run:
   ```text
   /fleet Investigate the checkout failure.

   Use separate read-only researchers for:
   1. Finding the calculation that produces the discount.
   2. Finding the test and expected result.
   3. Checking whether the database or migrations are involved.

   Do not change any files.
   Return one summary with file paths and line numbers.
   ```
2. Let the consolidated summary render. It should identify
   `demo/src/Store.Checkout/CheckoutCalculator.cs` as the authoritative calculation,
   that shipping is wrongly included in the discount base producing an $11 discount and
   $99 total, that the test expects $10/$100, and that persistence/migrations are not
   involved.

Say:

> "I'm using separate read-only researchers here because in a large codebase you don't
> want one model trying to hold the whole investigation in its head at once — splitting
> the work by concern and consolidating the findings is faster and less error-prone than
> one long serial exploration."

Hand off:

> "Now instead of losing this summary when the conversation ends, let's preserve it as
> evidence — that's the next demo."

---

## 22:00–27:00 — Demo 6: Evidence Checkpoint + Live Model Switching (wow #2)

File: `demo/playbooks/06-evidence-checkpoint.md`
Artifact: `demo/investigations/checkout-discount-checkpoint.md`

Say:

> "We already know the root cause from the fleet investigation — instead of letting
> that disappear the moment this chat ends, let's preserve it as durable evidence. And
> then let's prove something bigger: that a completely different model can pick up the
> work later using only that evidence, without re-investigating from scratch."

Demo steps:
1. Run:
   ```text
   /model
   ```
   Select **Claude Haiku 5.5** with **low** reasoning effort — a fast, cheap model.
2. Run:
   ```text
   Use the evidence-checkpoint skill to preserve the confirmed checkout findings in
   demo\investigations\checkout-discount-checkpoint.md.
   Include evidence, decisions, rejected hypotheses, test results, and the next action.
   Do not repeat the fleet investigation or change any other files.
   ```
3. Open the real checkpoint file afterward. Scroll through: the confirmed-evidence
   table (source-cited claims), the decisions table, the rejected-hypotheses table, the
   commands-executed table with real test/build/console output, and the "next action"
   line.
4. Confirm only one file changed:
   ```powershell
   git status --short
   ```
   Expect exactly: `M demo/investigations/checkout-discount-checkpoint.md`

Before the model switch, ask the room out loud:

> "Before I continue — if I now switch to a completely different, higher-reasoning
> model and tell it not to re-read any source files, only the checkpoint — do you think
> it can still tell me the exact fix?"

5. Run:
   ```text
   /model
   ```
   Select **GPT-6.1 Sol** with **high** reasoning effort.
6. Run:
   ```text
   Read only demo\investigations\checkout-discount-checkpoint.md.
   Using only its evidence, state the exact one-line fix and file path.
   Do not re-investigate, reread source files, or apply the change.
   ```

Say:

> "Notice the second model never opened `CheckoutCalculator.cs`, never re-ran a test,
> never re-read the instructions — it trusted the checkpoint written by a completely
> different model and got the exact right answer immediately. That's the whole point of
> an evidence checkpoint: it lets you switch models mid-investigation without paying the
> cost of re-exploration, and without losing precision to a vague summary."

Hand off:

> "We now have the fix confirmed by two different models, from a checkpoint, without
> either one needing to re-read the source. Let's actually apply it."

---

## 27:00–31:00 — Demo 7: Plan Mode Reveal

File: `demo/playbooks/07-plan-mode-reveal.md`

Say:

> "Okay, here's the trick. Here's what I want to change." The plan will generate a
> markdown file of the whole situation — the entire puzzle we want to solve. You keep
> interactively editing that, and only once you understand and verify everything
> written there do you proceed. It's usually a large file too, so that's typically the
> longest part of the process — just reading it through and making sure everything is
> understood."

Demo steps:
1. Switch to Plan mode. Run:
   ```text
   Use the checkpoint in demo\investigations\checkout-discount-checkpoint.md.
   Plan the fix for shipping incorrectly receiving the loyalty discount.
   Merchandise $100, shipping $10, discount 10%.
   Required discount $10 and total $100.
   Preserve public APIs and do not weaken tests. Do not apply changes yet.
   ```
2. **Before scrolling the generated plan**, ask the room: "What do you think it'll
   propose touching — just the calculator, or will it try to touch the tests too?"
3. Scroll the real generated plan live. Point at the file-change scope. It should scope
   to `demo/src/Store.Checkout/CheckoutCalculator.cs` only — consistent with the
   checkout-domain instructions and the `checkout-engineer` agent's own rule against
   weakening tests to get a pass.

Optional, only if presenting this demo standalone without Demos 5–6 before it, include
an Ask-mode side question first:
```text
Explain how the discount calculation works in CheckoutCalculator.cs.
```

4. Implement and verify:
   ```text
   Implement the approved plan and run demo\scripts\Verify-Fixed.ps1.
   Report the verification result.
   ```

Say:

> "Only once you understand and verify everything written there do you proceed. Then
> move into the implementation stage. Verify — if it doesn't work, go back into the
> planning phase. Do I need all of this every time? Probably not. If it's a small fix in
> one file, just do it in interactive mode, verify, and move on. If not, I'd recommend
> starting with Ask and Plan."

### Presenter checkpoint (not spoken) — after Demo 7

Restore `demo/investigations/checkout-discount-checkpoint.md` only if it now contains
changes you don't want to keep for future runs. If unsure, stop and check the diff
before overwriting:

```powershell
git diff -- demo/investigations/checkout-discount-checkpoint.md
git checkout -- demo/investigations/checkout-discount-checkpoint.md   # only if safe to discard
```

---

## 31:00–34:00 — Demo 8: Context Usage Audit (wow #3)

File: `demo/playbooks/08-context-usage-audit.md`

Say:

> "Another thing: context overhead. Have you ever used a context command — the usage
> command — to see what's inside your context? I used it once. I was just wondering
> why Copilot was taking so long to respond. Another thing — I had just started
> working, why was it already confused? More files with outdated guidance don't
> guarantee better output — in fact, the opposite. If you don't maintain them, if you
> don't review them from time to time, it has trouble understanding and
> troubleshooting.
>
> Also, context is working memory, and as the session grows, you have accumulating
> evidence that competes with source files, instructions, prior approaches, plans, and
> decisions. During a long investigation you'll notice Copilot forgetting what was found
> before, or trying to re-explore. It writes something vague instead of the precise fact
> or name — especially if you notice a lot of compaction happening in the process. That
> introduces a degraded process."

Demo steps (continue in the same checkout session — do not start a new one, the point
is that this session has real accumulated history from Demos 5–7):
1. Run:
   ```text
   /usage
   ```
   Read the numbers aloud: session duration, models used, token totals, edited lines.
2. Run:
   ```text
   /context
   ```
   Read: system/tool allocation, message-history allocation, free space.
3. Run:
   ```text
   What exact discount and total did Demo 5 show before the fix,
   and which test file proved it?
   ```
   Call out honestly whether the answer is precise (e.g. exact "$11 discount, $99
   total, CheckoutCalculatorTests.cs") or vague/re-exploring. Both outcomes are usable
   teaching moments — don't force the point if the answer is actually still precise.

---

### Presenter checkpoint (not spoken) — before Demo 9

Check `git status --short` again. If clean, reset the baseline the same way as before
Demo 5:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
```

---

## 34:00–37:00 — Demo 9: Copilot Fusion

File: `demo/playbooks/09-copilot-fusion.md`

Say:

> "So which models are actually the best? Honestly, there isn't one universal winner —
> it depends on the phase you're in. For architecture, security, or debugging, you want
> a high-reasoning model like Claude Sonnet or one of the higher-effort GPT variants —
> it's slower, but it actually thinks things through. For implementation, you switch to
> something faster, more codebase-aware — because at that point you don't need deep
> reasoning, you need speed and accuracy on what's already been planned. And for reviews
> or quick answers, when context is small, something lighter is enough — you're not
> asking it to explore the whole codebase, you're asking it to confirm something. Match
> the model to the task, not to habit.
>
> Now — what if you don't want to manually switch models for each phase? That's exactly
> what Copilot Fusion does. Instead of picking a model for each task, you choose Fusion,
> and it routes and orchestrates models for you — Single, where one model solves it
> directly; Cascade, where a fast model drafts and escalates to a stronger model if
> needed; or Critique, where one model drafts, another reviews, and the first revises.
> You just pick an effort level — Standard, High, or Max — and it decides the rest."

Demo steps:
1. Run:
   ```text
   /model
   ```
   Select **Copilot Fusion**.
2. Run a trivial prompt:
   ```text
   Say OK only.
   ```
   Point at the very first line of output — Fusion announces which model it picked and
   why, e.g. "Copilot Fusion selected GPT-6 Luna since the task has low complexity."
3. Run a harder prompt:
   ```text
   Investigate why the baseline checkout returns an $11 discount instead of $10.
   Propose the exact one-line fix without applying it.
   Do not modify files.
   ```
   Point out the complexity label changes (e.g. "medium complexity") and possibly a
   different model gets selected.

Note for presenting: Fusion's exact model choice and complexity label are decided live
each run — they may differ from a previous rehearsal. Don't claim a fixed outcome; let
the room see whatever it actually prints.

Say:

> "This is the model-matching instinct I just described, done automatically at runtime
> instead of me manually switching models myself. Worth trying if you're tired of
> switching models mid-task. One more thing before you try this yourselves — Copilot
> Fusion is a very recent feature, so make sure your CLI is updated (`copilot update`)
> or you may not see the routing banner at all."

---

## 37:00–39:00 — Demo 10: Skill Health Check

File: `demo/playbooks/10-skill-doctor-mention.md`

Say:

>"Architecture should be committed and maintained as a source of truth, ideally
> treated as a puzzle piece and kept maintainable alongside your codebase. The more
> drift it has from your codebase, the harder it is to understand. There's a nice skill
> called skill-doctor, which deals with exactly that situation of drifting state — we'll
> actually run that one live a bit later. Treat the instructions simply as instructions,
> like you'd treat a human — we sometimes follow them, sometimes we don't. Ideally,
> keep global instructions small."
 "One more quick one, and it's a different kind of check than everything else today —
> not about instructions contradicting each other, but about whether your skills
> themselves are well-formed."

Demo steps:
1. Run:
   ```text
   Check node --version, npm --version, and npx --version.
   If any are missing, stop and report what needs to be installed.
   ```
2. Run:
   ```text
   Run npx -y skill-doctor@latest .github\skills from the repository root.
   Report the score, errors, and warnings. Do not modify repository files.
   ```
3. Point at the score table — the two real skills in this repo, `grill-me` and
   `evidence-checkpoint`, should both report clean.

Say:

> "There's a tool called skill-doctor that checks whether your skills are well-formed —
> valid metadata, no broken file references, and a description clear enough that
> Copilot actually knows when to trigger it. It's a different check from the
> instruction-conflict demo earlier — this one is about skill quality, not contradictory
> rules."

---

## 39:00–40:00 — Close

Say:

> "So — instructions small and reviewed. The right mode for the task. Watch your
> context. The right model for the phase. That's really it."

---

## Bonus / time-permitting only — Demo 11: Copilot Memory

File: `demo/playbooks/11-copilot-memory.md`

Not scheduled inside the 40-minute slot — only run this if there's extra time in Q&A,
or mention it verbally and drop the link afterward:

> "One more thing if you want to go deeper afterward: Copilot Memory lets you store
> durable preferences — at the user level, like 'always ask before generating
> migrations,' or at the repository level, like where your EF Core migrations and
> DbContext actually live — so you don't have to restate them every single session."

If time allows:
```text
/memory on
```
```text
Remember these user-level preferences across all repositories:
- Prefer the smallest fix over unrelated refactoring.
- Explain the intended change before multi-file edits.
- Ask for approval before generating or applying database migrations.
- Run relevant tests and summarize results before proposing a commit.
Avoid duplicate memories. Report if memory storage is unavailable.
```
```text
Verify and remember these repository-level paths:
- EF Core migrations: demo\src\Store.Checkout.Persistence\Migrations
- DbContext: demo\src\Store.Checkout.Persistence\CheckoutDbContext.cs
Avoid duplicate memories. Report if memory storage is unavailable.
Do not modify files or generate or apply migrations.
```
```text
Confirm which memories were actually stored and their scopes.
Report any storage or verification limitations without claiming success.
```

Remove only demo-created memories afterward in Copilot Memory settings, so the account
used for future workshops starts clean.

---

## Full reset checklist (run after the workshop, or before a re-run)

```powershell
git status --short
# If clean and safe:
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Reset-Demo.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\demo\scripts\Verify-Baseline.ps1
git checkout -- demo/investigations/checkout-discount-checkpoint.md
```

Confirm the baseline line: `BASELINE VERIFIED: exactly 3 passed, 1 intended failure, 0 skipped.`
