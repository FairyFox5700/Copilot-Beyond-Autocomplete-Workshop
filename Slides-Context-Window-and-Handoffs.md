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

---

## Slide 67 — Switch Models When the Work Changes

One model does not have to handle every phase of a long task.

A practical pattern is:

```text
Fast model             → repository exploration and routine questions
Higher-reasoning model → architecture, ambiguity, security, hard debugging
Implementation model   → approved, well-bounded code changes
Review model           → independent diff and test review
```

Switch because the task changed—not because the current answer is merely slow.

Model availability, names, context tiers, and reasoning levels depend on the account and Copilot surface.

---

## Slide 68 — Copilot CLI: Switch Mid-Session

In an interactive GitHub Copilot CLI session:

```text
/model
```

Then select the model available to your account.

Important behavior:

- The default change is for the current session.
- If Copilot is idle, the selection takes effect immediately.
- If a turn is running, the switch is queued until that turn finishes.
- The selected model continues for the rest of the session unless changed again.
- Do not use a global or repository default change merely for a workshop experiment.

In VS Code, use the model picker in the chat input for the current conversation.

---

## Slide 69 — Real Workshop Example: Change Models by Phase

Use the file-upload scenario:

### Phase 1 — Explore with a fast model

```text
Inspect the repository for existing upload, storage, authorization,
and background-processing patterns. Do not edit files.
Record evidence in the investigation checkpoint.
```

### Phase 2 — Switch for difficult reasoning

```text
/model
```

Select an available higher-reasoning model, then ask:

```text
Use the grill-me and evidence-checkpoint skills.
Challenge the upload requirements, identify unresolved security and
failure decisions, and propose the smallest architecture.
Do not write code.
```

### Phase 3 — Switch for bounded implementation

After approving the plan, use `/model` again and select an appropriate implementation model:

```text
Implement only the approved first slice.
Run the agreed tests and update the checkpoint with results.
```

### Phase 4 — Optional independent review

Switch again or use a review agent:

```text
Review the final diff against the approved decision log.
Do not modify files. Report unsupported assumptions and missing tests.
```

---

## Slide 70 — Checkpoint Before Switching

Switching the model does not automatically repair weak context.

Before `/model`, record:

```text
Goal
Confirmed evidence
Approved decisions
Rejected hypotheses
Commands and results
Unresolved questions
Next action
```

Then give the next model this instruction:

```text
Read demo/investigations/file-upload-model-handoff.md and verify its
cited evidence before continuing. Do not reopen approved decisions
unless you find contradictory evidence.
```

This reduces repeated searches and prevents the new model from silently changing the contract.

---

## Slide 71 — When Not to Switch Models

Do not switch merely because:

- the answer is taking longer than expected;
- one attempt failed without diagnosing why;
- the task is a tiny, deterministic fix;
- the current model already has a coherent plan and evidence;
- the switch would add more coordination than value.

For the checkout discount defect, one model plus tests is enough.

For the upload feature—with authorization, content validation, quarantine, storage compensation, and asynchronous scanning—a deliberate model switch can be useful.

> Match the model to the phase, but preserve decisions and evidence outside the model.

---

## Slide 72 — Change Reasoning Effort When Complexity Changes

Some reasoning models expose multiple effort levels.

Use effort as a task-level control:

```text
Lower/default effort → exploration, lookup, formatting, routine edits
Higher effort        → ambiguity, architecture, security, concurrency,
                       difficult debugging, migration and failure analysis
```

Do not keep maximum effort enabled for every prompt. It can add latency and usage cost without improving deterministic work.

Availability and level names depend on the selected model and Copilot surface.

---

## Slide 73 — Copilot CLI and VS Code Controls

### Copilot CLI

In an interactive session:

```text
/model
```

1. Select a reasoning model.
2. Use the left and right arrow keys to change its reasoning level when options are available.
3. Press Enter to apply it to the current session.

If a turn is already running, the change applies after that turn finishes rather than changing the model midway through one response.

### VS Code

1. Open the model picker in the chat input.
2. Select a reasoning model.
3. Open the configuration arrow beside the model.
4. Select an available effort level.

Non-reasoning models do not expose this control.

---

## Slide 74 — Real Workshop Example: Effort by Phase

### Default effort — collect evidence

```text
Inspect the repository for upload, storage, authorization, and
background-processing patterns. Do not edit files. Cite exact paths.
```

This is mostly retrieval and classification. More reasoning is not automatically better.

### Higher effort — resolve the difficult decisions

Use `/model`, raise effort, then ask:

```text
Using the approved upload requirements, analyze tenant isolation,
content validation, quarantine, duplicate detection, partial storage
failures, compensation, and asynchronous scan states.
Find contradictions and unsafe assumptions. Do not write code.
```

### Default or moderate effort — implement the approved slice

```text
Implement only the approved provider-neutral first slice.
Follow the decision log and run the agreed tests.
Do not redesign the architecture.
```

### Higher effort — targeted final review

```text
Review the diff for authorization bypass, content-validation gaps,
partial-failure bugs, race conditions, and missing recovery tests.
Do not modify files.
```

---

## Slide 75 — When Higher Effort Is and Is Not Needed

Increase effort when:

- several valid designs have important trade-offs;
- a bug survives normal investigation;
- concurrency, authorization, money, or data loss is involved;
- a migration or rollback must remain compatible;
- multiple evidence sources conflict;
- the cost of a wrong answer is high.

Stay at default or lower effort when:

- locating files or references;
- summarizing a known diff;
- applying an approved mechanical change;
- formatting, renaming, or documentation work;
- running deterministic tests and builds;
- the task already has a precise contract and narrow scope.

> Increase reasoning for a specific difficult decision, then reduce it again for routine execution. Effort does not replace requirements, evidence, tests, or review.

---

## Slide 76 — Copilot CLI Equivalent of “Stash My Prompt”

GitHub Copilot CLI does not document Claude Code’s `Ctrl+S` prompt-stash behavior.

Use the Copilot-native controls instead:

```text
Ctrl+Q or Ctrl+Enter → queue the current input while the agent is busy
↑ on an empty prompt → recall the latest queued or steering prompt for editing
Ctrl+G               → edit a long draft in your external editor
Ctrl+X then /        → run a slash command without retyping the current draft
```

Queued entries show as pending and can be canceled before execution.

These queue and recall controls apply to local Copilot CLI sessions.

---

## Slide 77 — Queue, Recall, Edit, Requeue

While Copilot is working:

1. Draft the next prompt.
2. Press `Ctrl+Q` to queue it instead of interrupting the current turn.
3. If the current result changes your next step, wait until the input box is empty.
4. Press `↑` to recall the latest queued prompt.
5. Edit it and press `Ctrl+Q` again to requeue, or submit normally when Copilot is idle.

For a long prompt:

```text
Ctrl+G
```

Edit it in `$EDITOR`, save, and return to the CLI input.

> A queued prompt is scheduled work, not durable memory. Put decisions in the checkpoint file.

---

## Slide 78 — Real Workshop Example

Copilot is investigating the file-upload architecture. While it works, draft:

```text
Using the investigation results, propose the smallest provider-neutral
implementation and test plan. Do not write code until I approve.
```

Press:

```text
Ctrl+Q
```

The investigation discovers that malware scanning must be asynchronous. The queued prompt now needs a stronger constraint.

Recall it with `↑` and revise:

```text
Using the verified investigation checkpoint, propose the smallest
provider-neutral implementation and test plan.

Include quarantine and asynchronous scan states.
Do not expose file content before a clean scan.
Do not write code until I approve.
```

Requeue it or submit it after reviewing the investigation.

---

## Slide 79 — Change the Model Without Losing the Draft

If you have already typed the next prompt and decide the planning phase needs a different model:

```text
Ctrl+X then /
```

Run:

```text
/model
```

Select the model and effort level, then return to the draft instead of copying it into a temporary editor.

This is useful for:

- switching from exploration to architecture reasoning;
- increasing effort before security analysis;
- returning to a routine implementation model afterward.

The model change is queued if the current turn is still running.

---

## Slide 80 — When to Queue and When to Wait

Queue when:

- the next step is independent of the exact wording of the current answer;
- you are scheduling a checkpoint, test run, or summary;
- the task order is already approved;
- you want to avoid interrupting a long-running turn.

Wait or recall the prompt when:

- the next step depends on unresolved findings;
- approval is required before implementation;
- the agent has asked a question;
- the current result may change scope or architecture;
- the queued prompt could authorize an unsafe or premature action.

> Queue routine continuation. Do not queue irreversible decisions before reviewing the evidence.

## Sources

- GitHub Docs — Choosing a model in GitHub Copilot CLI: https://docs.github.com/en/copilot/how-tos/copilot-cli/use-copilot-cli/choose-a-model
- GitHub Docs — Copilot CLI command reference: https://docs.github.com/en/copilot/reference/copilot-cli-reference/cli-command-reference
- VS Code Docs — AI language models in VS Code: https://code.visualstudio.com/docs/agent-customization/language-models
