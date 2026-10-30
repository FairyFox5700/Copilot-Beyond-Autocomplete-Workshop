# Slide Ideas — Context Windows and Evidence-Backed Handoffs

## Slide 54 — The Context Window Is Working Memory

A coding agent does not retain unlimited investigation history.

As a session grows, useful evidence competes with:

- source files;
- instructions and skills;
- command output;
- failed approaches;
- plans and decisions;
- tool definitions and conversation history.

> Treat conversation context as temporary working memory, not as the investigation record.

---

## Slide 55 — What Information Loss Looks Like

During a long investigation, an agent may:

- forget an earlier finding;
- compress a precise fact into a vague summary;
- repeat searches and commands;
- lose the rationale behind a decision;
- reintroduce a rejected approach;
- disagree with another agent that saw different evidence.

The failure is often invisible: the answer remains fluent even when the evidence chain is incomplete.

---

## Slide 56 — Real Workshop Example: A Small Bug, Several Evidence Sources

The checkout defect touches several forms of context:

```text
Repository instructions  → where business rules belong
Domain code              → actual calculation
Focused tests            → expected behavior
Console application      → visible result
Persistence project      → proves no migration is needed
Commands                 → red and green evidence
```

Without a durable record, a later agent might:

- edit the console output instead of the domain;
- weaken the test;
- propose an unnecessary migration;
- forget that shipping is explicitly non-discountable.

---

## Slide 57 — Strategy 1: Use a Structured Scratchpad

Real workshop artifact:

```text
demo/investigations/checkout-discount-checkpoint.md
```

It records:

- investigation goal;
- repository state;
- confirmed evidence;
- decisions and rationale;
- rejected hypotheses;
- command results;
- unresolved risks;
- one next action.

**Important:** a scratchpad is not a stream-of-consciousness log. It is a maintained evidence ledger.

---

## Slide 58 — Real Evidence Ledger

```markdown
| ID | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Shipping is included in the discount base. |
  CheckoutCalculator.cs:24-29 | confirmed |
| E2 | Shipping is never discountable. |
  checkout-domain.instructions.md:7-9 | confirmed |
| E3 | The test expects discount $10 and total $100. |
  CheckoutCalculatorTests.cs:9-16 | confirmed |
```

Why this survives context loss:

- Claims are concise.
- Evidence can be reopened.
- Status separates facts from hypotheses.
- Another agent can verify rather than trust the summary.

---

## Slide 59 — Strategy 2: Checkpoint Before Phase Changes

Create a checkpoint:

- after a material discovery;
- after scope changes;
- before moving from investigation to implementation;
- before compaction or a fresh session;
- before handing work to another agent;
- after tests change the confidence of a claim.

A useful checkpoint answers:

```text
What do we know?
How do we know it?
What did we decide?
What remains unresolved?
What is the next action?
```

---

## Slide 60 — Strategy 3: Split Research Across Subagents

Do not ask several agents to investigate the same vague problem.

Use non-overlapping evidence questions:

```text
Agent A: Find the authoritative checkout calculation.
Agent B: Identify tests and exact expected behavior.
Agent C: Determine whether persistence or migrations are affected.
```

Require every agent to return:

1. conclusion;
2. exact file and line evidence;
3. commands and results;
4. unresolved questions;
5. recommended next action;
6. files changed, or `none`.

Use read-only agents during investigation unless isolated worktrees are available.

---

## Slide 61 — Subagent Output Is a Claim, Not Proof

Example conflict:

```text
Agent A: Only CheckoutCalculator must change.
Agent C: A database migration may be required.
```

Resolution:

- Inspect the call flow.
- Check whether persistence participates in the calculation.
- Review the DbContext and migration.
- Run the focused tests.

Real conclusion:

```text
CheckoutDbContext stores order metadata but does not calculate totals.
No schema change is required.
```

> Prefer authoritative source code and executable evidence over agent confidence or majority opinion.

---

## Slide 62 — Strategy 4: Evidence-Backed Handoffs

Weak handoff:

```text
I investigated checkout. It is probably a rounding problem.
Continue fixing it.
```

Strong handoff:

```text
Goal: Correct loyalty checkout total.
Confirmed: Discount base includes shipping at CheckoutCalculator.cs:24-29.
Contract: Shipping is non-discountable.
Rejected: Rounding and migration hypotheses.
Baseline: 3 passed, 1 intended failure.
Next: Change only discountBase, then run focused tests, full tests,
build, console check, and diff review.
```

---

## Slide 63 — Strategy 5: Keep Large Outputs Out of the Conversation

Avoid pasting thousands of lines of logs into chat.

Instead:

1. save or retain the complete output;
2. record its location;
3. extract the decisive lines;
4. link the evidence to a claim;
5. preserve the command and exit result.

This keeps context available for reasoning while maintaining a path back to the full evidence.

---

## Slide 64 — Real Reusable Workflow

Workshop assets:

```text
.github/skills/evidence-checkpoint/SKILL.md
.github/skills/evidence-checkpoint/templates/investigation-checkpoint.md
.github/prompts/checkpoint-investigation.prompt.md
demo/investigations/checkout-discount-checkpoint.md
```

Invoke:

```text
/checkpoint-investigation

Investigate the checkout loyalty defect. Do not edit production code.
```

The workflow requires a scratchpad, evidence, verified subagent results, a checkpoint, and an approval-ready next action.

---

## Slide 65 — When to Continue and When to Start Fresh

Continue the current session when:

- the evidence remains coherent;
- the task is still within the same phase;
- prior context materially helps the next step.

Create a checkpoint and start fresh when:

- searches or explanations repeat;
- rejected approaches reappear;
- decisions are forgotten;
- the task moves from investigation to implementation;
- the session contains large irrelevant logs;
- several agents need a common source of truth.

A fresh session plus a strong checkpoint is often safer than a long session plus a weak memory.

---

## Slide 66 — Workshop Takeaway

```text
Conversation context = temporary working memory
Scratchpad           = durable investigation state
Evidence             = verifiable facts
Checkpoint           = phase boundary
Subagent             = bounded research worker
Handoff              = restartable continuation contract
```

> The goal is not to make the agent remember everything. The goal is to make the work recoverable when it does not.
