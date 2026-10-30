# GitHub Copilot Beyond Autocomplete — Complete Slide Source

A single Markdown source for the workshop deck. The runnable project and presenter script live beside this file.

## Deck map

- **Slides 1–17:** Copilot instructions, scope, agents, and the real repository structure
- **Slides 18–31:** Ask → Plan → Implement → Verify → Review
- **Slides 32–41:** Grill-me requirements interview and reusable skills
- **Slides 42–53:** Instruction hierarchy and monorepo risks
- **Slides 54–100:** Context, handoffs, model switching, effort, prompt queueing, usage, skills, compact, and clear
- **Slides 101–113:** GitHub Copilot Memory with real project paths and best practices

---

## Slide 1 — What Are GitHub Copilot Instructions?

- Persistent guidance that helps Copilot understand a repository
- Applied automatically when relevant
- Useful for architecture, conventions, safety rules, and validation
- Different from a one-time chat prompt
- Guidance, not deterministic enforcement

---

## Slide 2 — Instruction Levels

- **Organization:** shared company standards
- **Repository:** rules for the whole application
- **Path-specific:** guidance for selected files or folders
- **User/profile:** personal preferences, depending on the Copilot surface
- **Session prompt:** temporary context, not a persistent instruction file

---

## Slide 3 — Repository-Wide Instructions

Path:

```text
.github/copilot-instructions.md
```

Use it for rules relevant to most tasks: architecture boundaries, C# conventions, security expectations, testing, and completion checks.

---

## Slide 4 — What Belongs in Instructions?

- Repeated repository rules
- Decisions Copilot cannot reliably infer
- Architectural boundaries
- Known safety constraints
- Required tests and validation
- Mistakes the team has already encountered

```markdown
- Keep business logic out of controllers.
- Never claim a build passed unless it was executed.
```

---

## Slide 5 — What Should Stay Out?

- Generic advice such as “write clean code”
- Complete repository documentation
- Temporary feature requirements
- Large folder listings Copilot can inspect
- Duplicated README content
- Secrets or credentials
- Rules relevant to only one path
- Requirements that need technical enforcement

---

## Slide 6 — Strong Instruction Pattern

```text
Trigger → Investigation → Action → Verification
```

Weak:

```text
Write safe database code.
```

Strong:

```text
When changing the schema, inspect existing migrations, create new
history instead of rewriting deployed migrations, review Up and Down,
and report compatibility risks and validation commands.
```

---

## Slide 7 — Keep Global Instructions Small

Before adding a rule, ask:

1. Is it relevant to most tasks?
2. Can Copilot infer it from current code?
3. Does it prevent a real recurring mistake?
4. Would path-specific guidance be narrower?
5. Does it need tests, CI, permissions, or policy instead?

---

## Slide 8 — Path-Specific Instructions

Location:

```text
.github/instructions/*.instructions.md
```

Use `applyTo` to make scope explicit:

```yaml
---
applyTo: "demo/tests/**/*.cs"
---
```

---

## Slide 9 — Test-Specific Example

```markdown
---
applyTo: "demo/tests/**/*.cs"
---

- Use xUnit and Arrange–Act–Assert.
- Use exact decimal assertions for money.
- Avoid sleeps, network calls, and shared mutable state.
- Do not weaken assertions to make tests pass.
```

---

## Slide 10 — Migration-Only Example

```markdown
---
applyTo: "demo/src/Store.Checkout.Persistence/Migrations/**/*.cs"
---

- Treat existing migrations as immutable history.
- Review both Up() and Down().
- Call out destructive operations.
- Do not generate or apply migrations unless explicitly requested.
```

---

## Slide 11 — API-Specific Example

```markdown
---
applyTo: "**/Controllers/**/*.cs"
---

- Keep business logic outside controllers.
- Preserve the established error contract.
- Return correct HTTP status codes.
- Do not expose exception details or sensitive information.
```

---

## Slide 12 — How Instructions Combine

When Copilot edits a test:

```text
Repository-wide rules + test-specific rules
```

When it edits a migration:

```text
Repository-wide rules + migration-specific rules
```

Unrelated path-specific rules should not apply.

---

## Slide 13 — User-Level vs Repository-Level Guidance

**User-level:** personal communication and workflow preferences across projects.

**Repository-level:** shared architecture, build, testing, and safety requirements committed with the project.

> Personal preferences belong to the user; team engineering rules belong to the repository.

---

## Slide 14 — Instructions vs Custom Agents

| Instructions | Custom agent |
|---|---|
| Add rules and context | Defines a specialist role |
| Applied when relevant | Selected or delegated to |
| Good for conventions | Good for review, testing, planning |
| Repository-wide or path-specific | Can restrict tools and use focused context |

---

## Slide 15 — When to Use Each

Use **instructions** for architecture rules, C# conventions, migration safety, test conventions, and completion requirements.

Use a **custom agent** for a read-only reviewer, security specialist, test engineer, planner, or documentation role with a dedicated toolset.

---

## Slide 16 — Workshop Repository Structure

```text
.github/
├── copilot-instructions.md
├── instructions/
├── agents/
├── prompts/
└── skills/
demo/
├── src/
├── tests/
├── scripts/
├── scenarios/
└── investigations/
```

---

## Slide 17 — Instruction Takeaway

> Put shared durable rules in repository instructions. Put specialized rules in path-specific files. Use agents for specialized roles. Use tests and policy for enforcement.

---

## Slide 18 — Daily Copilot Workflow

```text
ASK → PLAN → IMPLEMENT → VERIFY → REVIEW
```

Workshop defect: a loyalty discount incorrectly includes shipping, producing an $11 discount and $99 total instead of $10 and $100.

---

## Slide 19 — Ask and Investigate

```text
Investigate why checkout returns $99.
Do not change files.
Trace the calculation, identify the authoritative implementation,
relevant instructions, tests, and unresolved assumptions.
```

Expected finding: `CheckoutCalculator` includes shipping in the discount base.

---

## Slide 20 — Plan the Change

```text
/plan Correct the checkout loyalty discount.
Shipping is never discountable.
Preserve the public API.
Name changed files, tests, risks, and verification commands.
Do not implement yet.
```

Use planning when a wrong direction would be expensive, not for every trivial edit.

---

## Slide 21 — What Makes a Good Plan?

A useful plan identifies:

1. authoritative layer;
2. files to change;
3. files that must not change;
4. patterns to follow;
5. tests;
6. compatibility and security risks;
7. verification commands;
8. unresolved assumptions.

---

## Slide 22 — Implement Incrementally

```text
Implement the approved checkout fix.
Calculate the discount from merchandise only.
Preserve the public API and test expectations.
Make the smallest necessary production change.
```

Expected change:

```csharp
var discountBase = merchandiseSubtotal;
```

---

## Slide 23 — Small Fix or Planned Change?

Skip a separate plan when the defect, target, behavior, tests, and rollback are clear.

Use Plan mode when requirements are ambiguous, several layers are affected, compatibility matters, or security and data changes are involved.

---

## Slide 24 — Verify with Evidence

```text
Run focused tests, full tests, build, and the application.
Report exact commands, passes, failures, and checks not executed.
Do not claim completion if validation fails.
```

---

## Slide 25 — Verification Ladder

```text
Focused tests
    ↓
Full test suite
    ↓
Solution build
    ↓
Visible application result
    ↓
Final diff
```

Corrected state: 4 tests pass, build succeeds, discount is $10, total is $100.

---

## Slide 26 — Review Before Committing

```text
Review the diff for correctness, security, unintended API changes,
and missing tests. Do not modify files. Report findings with paths
and line references.
```

Confirm that no tests were weakened and no migration was changed.

---

## Slide 27 — Completion Report

Include:

- files changed;
- behavior corrected;
- commands executed;
- test and build results;
- visible runtime result;
- files intentionally not changed;
- remaining risks and unverified checks.

---

## Slide 28 — Interactive Mode vs Prompt Mode

Use interactive mode when requirements need clarification or intermediate decisions matter.

Use:

```text
copilot -p "..."
```

for bounded, repeatable tasks with clear expected output.

---

## Slide 29 — Prompt-Mode Examples

```bash
copilot -p "Review the current diff. Do not modify files."
copilot -p "Analyze the .NET build errors and propose the smallest fix."
copilot -p "Summarize what changed, which checks passed, and remaining risks."
```

---

## Slide 30 — Be Explicit About Permissions

For analysis-only work:

- avoid write permissions;
- say files must not change;
- request file and line evidence.

For implementation:

- allow only required tools;
- limit scope;
- run tests;
- inspect the diff yourself.

---

## Slide 31 — Complete Workshop Flow

```text
ASK        Investigate why checkout returns $99
PLAN       Identify authoritative fix, scope, and tests
IMPLEMENT  Exclude shipping from the discount base
VERIFY     Run tests, build, application, and diff
REVIEW     Confirm no unrelated or unsafe changes
```

> Copilot should investigate, follow repository guidance, make a focused change, verify it, and provide evidence for human review.

---

## Slide 32 — Give Copilot a Deliberately Vague Feature

```text
Build an API endpoint that lets users upload files.
```

- Familiar C# feature
- Sounds small
- Hides product, security, storage, and operational decisions
- Do not let Copilot implement immediately

**Workshop action:** invoke the `grill-me` project skill and paste the request.

---

## Slide 33 — Let Copilot Interview You

**Copilot:**

> What types of files should the endpoint accept, and what is the maximum file size? “Any file” is not sufficient because it affects validation, storage, and security.

**You:**

> PDF and PNG files, up to 10 MB.

**Copilot:**

> How will the application verify that the content is actually a PDF or PNG rather than trusting a client-supplied MIME type or filename extension?

**Teaching point:** every answer changes the implementation and tests.

---

## Slide 34 — Questions That Change the Architecture

Continue the interview into:

- Who may upload files?
- Who may read or delete them?
- How is tenant ownership enforced?
- What counts as a duplicate upload?
- Where are files and metadata stored?
- What happens when storage succeeds but metadata persistence fails?
- Is malware scanning synchronous, asynchronous, or absent?
- Are files quarantined before scanning completes?
- What status codes represent accepted, rejected, duplicate, and failed uploads?
- Which success, boundary, failure, and abuse cases require tests?

---

## Slide 35 — Example Decision Chain

```text
PDF and PNG only
        ↓
Content signatures must be inspected
        ↓
10 MB maximum
        ↓
Request-size and streaming limits
        ↓
Authenticated owners only
        ↓
Authorization and tenant tests
        ↓
Malware scan before availability
        ↓
Quarantine state and asynchronous workflow
```

**Teaching point:** requirements discovery is architecture work.

---

## Slide 36 — Prompt vs Instructions vs Skill

### A. One-off prompt

```text
Interview me before implementing this feature.
```

- Fast to try
- Easy to forget
- Quality varies between tasks

### B. Repository instructions

```text
For non-trivial features, clarify security, failure behavior,
and acceptance criteria before implementation.
```

- Broad, durable team rule
- Loaded with many repository tasks
- Should remain concise

### C. Reusable skill

```text
.github/skills/grill-me/SKILL.md
```

- Named multi-step workflow
- Loaded when relevant
- Carries its own interview, decision-log, planning, and verification process

---

## Slide 37 — What the Grill-Me Skill Packages

- Repository inspection before questions
- One decision-changing question at a time
- Rejection of vague requirements
- Security and authorization questions
- Failure and recovery questions
- Test and observability questions
- Decision log
- Unresolved-assumption register
- Smallest implementation proposal
- Approval gate before coding
- Verification after implementation

**Key distinction:** a skill is not merely a longer prompt; it packages repeatable behavior.

---

## Slide 38 — Run the Workshop Demo

Start with:

```text
Use the grill-me skill.

Build an API endpoint that lets users upload files.
```

Presenter answers:

```text
PDF and PNG files, up to 10 MB.
```

Continue until Copilot covers:

1. content verification;
2. authorization and ownership;
3. duplicate handling;
4. storage and partial failures;
5. malware scanning and quarantine;
6. test cases.

---

## Slide 39 — Stop Before Code

After the interview, submit:

```text
Now summarize the decisions we made.

Identify assumptions I failed to resolve.
Propose the smallest implementation and a test plan.
Name the files that should and should not change.
Do not write code until I approve.
```

Expected output:

- Decision log
- Unresolved assumptions
- Minimal implementation scope
- Test plan
- Verification commands
- Explicit approval request

---

## Slide 40 — Approve, Implement, Verify

After reviewing the contract:

```text
I approve the implementation contract.

Implement the smallest cohesive change.
Follow repository and path-specific instructions.
Run the approved tests and build.
Review the final diff and report remaining risks.
```

Verification ladder:

```text
Focused tests → integration tests → build → runtime check → diff review
```

---

## Slide 41 — Workshop Takeaway

> A prompt asks for an answer. Instructions shape recurring behavior. A skill packages a reusable workflow.

The `grill-me` skill changes the agent from:

```text
Request → Generate code
```

into:

```text
Request → Investigate → Interview → Decide → Plan → Approve → Implement → Verify
```

---

## Slide 42 — More Instructions Do Not Mean More Intelligence

Multiple `AGENTS.md` files—or equivalent scoped instruction files—can help in a monorepo.

They can also make an agent less predictable when they introduce:

- conflicting rules;
- unnecessary context;
- unclear scope;
- duplicated guidance;
- maintenance and debugging overhead.

> Add another instruction file only when a directory genuinely needs different guidance.

---

## Slide 43 — Problem 1: Instruction Conflicts

```text
Root AGENTS.md:       Use xUnit.
src/AGENTS.md:        Use NUnit.
tests/AGENTS.md:      Use MSTest.
```

Possible outcomes:

- The agent follows the wrong rule.
- It combines incompatible frameworks.
- Different files receive inconsistent tests.
- The result changes between Copilot surfaces.

**Workshop question:** Which instruction is authoritative, and why are test-framework rules in `src/` at all?

---

## Slide 44 — Problem 2: Context Overhead

```text
Root instructions
+ backend instructions
+ frontend instructions
+ test instructions
+ feature instructions
+ task prompt
+ source code
+ command output
```

Every relevant instruction consumes context before the agent solves the task.

Consequences:

- Less room for source code and requirements
- Important evidence competes with generic guidance
- Higher chance of missing a critical constraint
- More cost and latency

**Rule:** keep always-loaded guidance short; load specialized workflows only when relevant.

---

## Slide 45 — Problem 3: Unclear Scope

```text
repo/
├── AGENTS.md
├── src/
│   ├── AGENTS.md
│   └── API/
│       ├── AGENTS.md
│       └── Controllers/
└── tests/
    └── AGENTS.md
```

Ask:

- Which instructions apply to a controller test?
- What happens when one task changes API and test files?
- Which rule wins when two files disagree?
- Does the current IDE, CLI, or cloud agent discover nested files identically?

> Do not assume every coding-agent surface discovers, combines, or prioritizes nested instruction files in the same way.

---

## Slide 46 — Prefer Explicit Path Scope

For GitHub Copilot, make specialized scope visible when the surface supports path-specific instructions:

```markdown
---
applyTo: "demo/tests/**/*.cs"
---

- Use xUnit.
- Use exact decimal assertions for money.
```

Benefits:

- The intended scope is reviewable.
- The rule is attached to matching files.
- Repository-wide instructions remain small.
- Teams can search for all scoped rules in one directory.

**Caution:** support and discovery details can differ between Copilot surfaces; verify the client used in the workshop.

---

## Slide 47 — Problem 4: Duplication and Drift

A naming rule is copied into:

```text
AGENTS.md
src/AGENTS.md
tests/AGENTS.md
```

Six months later, only one copy is updated.

Now the agent receives different guidance depending on where it works.

**Better pattern:**

- Keep shared rules in one authoritative file.
- Keep scoped files limited to genuine differences.
- Link to maintained documentation instead of copying large sections.
- Review instruction changes like production code.

---

## Slide 48 — Problem 5: False Confidence

More instruction files do not guarantee better output.

The agent may still:

- follow outdated architecture guidance;
- treat an example as a mandatory rule;
- apply a local convention outside its scope;
- miss a file that was not loaded;
- follow instructions but misunderstand the task.

> Instructions guide model behaviour. Tests, builds, permissions, CI, and review provide evidence and enforcement.

---

## Slide 49 — Problem 6: Harder Troubleshooting

When Copilot produces incorrect code, investigate:

1. Which instruction files were available?
2. Which ones were actually referenced?
3. Did applicable rules contradict one another?
4. Was the expected scope supported by this Copilot surface?
5. Was the instruction too vague or outdated?
6. Did deterministic validation catch the mistake?

**Debugging technique:** ask Copilot to list the instructions relevant to the current file, then verify the answer against the repository and client UI.

---

## Slide 50 — Recommended Monorepo Starting Point

```text
repo/
├── AGENTS.md or .github/copilot-instructions.md
├── src/
│   ├── API/
│   └── Worker/
├── tests/
└── docs/
```

Start with one root file containing universal rules.

Add scoped guidance only when API, worker, tests, migrations, or another directory genuinely needs different behavior.

| Root instructions | Scoped instructions |
|---|---|
| Architecture principles | API-specific conventions |
| General C# conventions | Worker-specific conventions |
| Standard build and test commands | Local validation commands |
| Security principles | Framework-specific patterns |
| Shared coding standards | Directory-specific constraints |

---

## Slide 51 — Workshop Project: Deliberate Scope

```text
.github/
├── copilot-instructions.md
└── instructions/
    ├── checkout-domain.instructions.md
    ├── tests.instructions.md
    └── migrations.instructions.md
```

Why this structure works:

- Global file: .NET version, money, API stability, completion checks
- Domain file: checkout invariants only
- Test file: xUnit and deterministic test conventions only
- Migration file: EF Core history and deployment safety only

The same rule is not copied into every file.

---

## Slide 52 — Decision Checklist Before Adding a File

Ask:

1. Does this guidance apply to most repository tasks?
   - Put it in the root instructions.
2. Does it apply only to a clear path or file type?
   - Use a scoped instruction.
3. Is it a multi-step workflow used occasionally?
   - Use a skill or prompt file.
4. Is the information already discoverable from code or documentation?
   - Do not duplicate it.
5. Does the rule need deterministic enforcement?
   - Use tests, analyzers, CI, permissions, or hooks.
6. Can we explain which file is authoritative if two rules disagree?
   - If not, simplify before adding another file.

---

## Slide 53 — Participant Exercise: Audit the Hierarchy

Given:

```text
Root:  Use xUnit.
src:   Use NUnit.
tests: Use MSTest.
```

Ask participants to:

1. identify every conflict;
2. choose one authoritative shared rule;
3. remove duplicated guidance;
4. scope only genuine local differences;
5. define the test command that proves the result;
6. explain how they would verify which instructions Copilot used.

**Takeaway:** instruction architecture should reduce decisions, not create new ones.

---

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

---

## Slide 81 — Usage and Context Are Different Questions

Use separate commands because they answer different questions:

```text
/usage    → What has this session consumed?
/context  → What is occupying the current context window?
```

`/usage` reports session metrics such as duration, edited lines, per-model token totals, and applicable AI-credit consumption.

`/context` visualizes token allocation, including system/tools, message history, free space, and buffer.

High usage does not necessarily mean the current context is full, and a crowded context does not tell you the total session cost.

---

## Slide 82 — Inspect Usage

Run:

```text
/usage
```

Look for:

- models used during the session;
- input and output token totals by model;
- session duration;
- lines of code edited;
- AI credits when applicable to the account.

Use it to answer:

- Did a model switch change consumption?
- Did a high-effort phase dominate usage?
- Is a routine task using more capability than necessary?

Do not use usage totals as proof of output quality.

---

## Slide 83 — Inspect the Context Window

Run:

```text
/context
```

Analyze:

- **System/tools:** static runtime instructions and available tool definitions
- **Message history:** prompts, responses, tool output, and summaries
- **Free space:** remaining capacity for new evidence and work
- **Buffer:** space reserved for safe operation and output

Warning signs:

- large static context before meaningful work begins;
- message history dominated by logs or failed attempts;
- little free space before implementation starts;
- repeated compaction without a durable checkpoint.

---

## Slide 84 — Inspect What Can Influence the Session

Use dedicated commands:

| Question | Command |
|---|---|
| Which instruction files are enabled? | `/instructions` |
| Which skills are available? | `/skills list` |
| What does one skill contain and where is it from? | `/skills info SKILL-NAME` |
| What is the current session state? | `/session` |
| What summaries were created by compaction? | `/session checkpoints` |
| What is in one checkpoint? | `/session checkpoints NUMBER` |
| Which temporary artifacts exist? | `/session files` |
| What plan is active? | `/session plan` |

`/context` shows allocation. These commands help explain what contributed to the session.

---

## Slide 85 — Real Workshop Context Audit

At the beginning of the checkout workshop, run:

```text
/usage
/context
/instructions
/skills list
/skills info grill-me
/skills info evidence-checkpoint
/session
```

Ask participants:

1. Is this a fresh or already expensive session?
2. How much context is static versus conversation history?
3. Which instruction files are relevant to the checkout defect?
4. Do we need either skill for a one-line deterministic fix?
5. Are any skills or instructions duplicated?
6. Should we continue this session or start a focused one?

Do not compare exact token numbers across attendees; model, client, configuration, and available tools differ.

---

## Slide 86 — How Skills Affect Context

Skills are discovered by name and description. When Copilot chooses or you invoke a skill, its `SKILL.md` instructions are injected into the session context.

Skills become expensive or distracting when:

- descriptions are so broad that many skills appear relevant;
- several skills are invoked for one small task;
- skill bodies are long and duplicate repository instructions;
- large references are loaded when only one rule is needed;
- multiple skills prescribe conflicting workflows;
- a skill remains part of a long session after its phase has ended.

Not every discovered skill necessarily injects its full body. The problem is uncontrolled selection and oversized skill content, not merely the existence of files on disk.

---

## Slide 87 — Real Example: Useful Skill vs Skill Overload

### Appropriate for the file-upload feature

```text
grill-me            → requirements interview
evidence-checkpoint → long investigation and model handoff
```

Each skill owns a different phase.

### Excessive for the checkout defect

```text
grill-me
evidence-checkpoint
migration-review
security-review
release-handoff
architecture-planner
```

The defect already has:

- an explicit business rule;
- one failing focused test;
- one authoritative calculation;
- deterministic verification.

Loading several workflows adds instructions without adding evidence.

---

## Slide 88 — Skill Hygiene: Do

- Give every skill a narrow trigger in its description.
- Keep `SKILL.md` focused on the reusable procedure.
- Put optional detail in supporting references.
- Avoid duplicating repository-wide instructions.
- Use `/skills info NAME` before trusting an unfamiliar skill.
- Disable skills that are not appropriate for the current repository or session.
- Use one skill per distinct need rather than stacking overlapping workflows.
- Recheck `/context` after enabling or invoking substantial customizations.
- Keep a durable checkpoint before compaction or a fresh session.

---

## Slide 89 — Skill Hygiene: Do Not

- Do not install every popular skill “just in case.”
- Do not use descriptions such as “use for all coding tasks.”
- Do not copy the same standards into instructions, agents, prompts, and skills.
- Do not bundle unrelated workflows into one giant skill.
- Do not invoke multiple reviewers with identical responsibilities.
- Do not paste complete logs and documents into a skill body.
- Do not assume disabling a skill erases content already present in conversation history.
- Do not compact blindly and trust a lossy summary as the only evidence record.

---

## Slide 90 — Compact Deliberately

Copilot CLI manages context automatically, but manual compaction is available:

```text
/compact focus on the verified upload decisions, evidence,
unresolved risks, and approved next action
```

Before compacting:

1. update the repository checkpoint;
2. record decisions and rejected hypotheses;
3. preserve decisive command results;
4. identify the next action.

After compacting:

```text
/session checkpoints
/session checkpoints NUMBER
```

Review the generated summary. If a critical decision disappeared, repair the durable checkpoint rather than relying on conversation memory.

---

## Slide 91 — Start Fresh for Unrelated Work

Use a new or cleared session when:

- the next task is unrelated;
- skills and instructions from the previous phase are no longer useful;
- old failures dominate the message history;
- several compactions have made the reasoning difficult to audit;
- a strong handoff already contains everything needed to continue.

A practical sequence:

```text
1. Update evidence checkpoint
2. Inspect /usage and /context
3. Review /instructions and /skills list
4. Disable irrelevant customizations if needed
5. Start a focused session
6. Load only the checkpoint and relevant files
```

> Context management is not about chasing the smallest token count. It is about preserving the highest-signal evidence for the current decision.

---

## Slide 92 — Compact, Clear, and External State Solve Different Problems

Do not treat them as interchangeable:

```text
/compact       → Same task, smaller active conversation
/clear or /new → Different task, fresh conversation
External state → Knowledge must survive conversations and sessions
```

The decision is about continuity:

- Do I still need the current reasoning chain?
- Is the next task related?
- Must another session or person recover these decisions?

---

## Slide 93 — Compact: Continue the Same Task

```text
Large active conversation
          ↓
/compact focus on verified findings and next action
          ↓
Compressed conversation summary
          ↓
Continue the same investigation
```

Use compaction when:

- the goal has not changed;
- accumulated history is crowding the window;
- earlier evidence still matters;
- you want to continue in the same session.

Before compacting, update external evidence. Afterward, inspect:

```text
/session checkpoints
/session checkpoints NUMBER
```

Compaction is lossy summarization, not durable project memory.

---

## Slide 94 — Clear: Start a Different Task

GitHub Copilot CLI supports:

```text
/clear
/new
/reset
```

These start a new conversation. You can optionally include the next prompt:

```text
/clear Review the checkout migration for destructive operations.
```

Use a fresh conversation when:

- the previous task is complete;
- the new task is unrelated;
- old instructions, failures, or decisions would distract the agent;
- you have already preserved anything worth keeping.

`Ctrl+L` only clears the terminal display. It does not create a fresh conversation.

---

## Slide 95 — External State: Preserve Knowledge

Use repository or shared artifacts when knowledge must survive the active conversation:

```text
.github/copilot-instructions.md             → durable repository rules
.github/instructions/*.instructions.md      → path-specific rules
.github/skills/*/SKILL.md                    → repeatable workflows
docs/adr/*.md                               → architectural decisions
demo/investigations/*.md                    → evidence and investigation state
tests and verification scripts              → executable expectations
issues or pull requests                      → team-visible work history
```

Do not put temporary investigation details into global instructions. Promote only stable, reusable knowledge.

---

## Slide 96 — Copilot Decision Table

| Scenario | Best choice | Why |
|---|---|---|
| Same bug investigation, context is crowded | Checkpoint + `/compact` | Preserve evidence and continue the same task |
| Feature implementation runs for several hours | External checkpoint + periodic compact | Decisions must survive lossy summaries |
| Feature A is complete; Feature B is unrelated | Save useful state, then `/clear` or `/new` | Prevent irrelevant history from influencing the new task |
| Monorepo investigation continues across days | External evidence ledger or ADR | Findings must survive sessions and be reviewable |
| Huge files or lengthy command output | Selective reading or bounded subagent | Avoid filling the main context with low-signal text |
| Architecture decision must guide future work | ADR or concise repository instructions | Durable, versioned project knowledge |
| Separate subsystem needs research | Read-only subagent with evidence contract | Isolate exploration and return concise findings |

---

## Slide 97 — Real Example: Checkout Defect

The checkout defect is small and deterministic.

Best workflow:

```text
1. Run the focused failing test.
2. Inspect CheckoutCalculator.cs.
3. Apply the one-line fix.
4. Run the verification ladder.
```

Do not compact merely because `/context` shows some usage.

After completion, starting an unrelated migration review:

```text
/clear Review the checkout migration for destructive operations.
Do not modify or apply migrations.
```

The previous defect history is not useful to the new review.

---

## Slide 98 — Real Example: File-Upload Feature

The upload feature spans requirements, security, storage, asynchronous scanning, and failure recovery.

Use all three mechanisms deliberately:

```text
External state:
  demo/investigations/file-upload-model-handoff.md

Same long task:
  /compact focus on approved upload decisions, evidence,
  unresolved risks, and the next action

Unrelated next task:
  /clear Investigate checkout performance independently.
```

The handoff remains authoritative if a compacted summary omits a detail.

---

## Slide 99 — What to Save Externally

Save:

- confirmed findings with exact evidence;
- decisions and rationale;
- rejected hypotheses;
- approved scope and non-goals;
- commands executed and exact results;
- unresolved risks;
- the next action;
- stable architecture decisions worth reusing.

Do not save as durable guidance:

- raw chain-of-thought or speculative reasoning;
- obsolete hypotheses without a rejected status;
- enormous unfiltered logs;
- secrets or sensitive data;
- one-off task details in global instruction files;
- claims that were never verified.

---

## Slide 100 — Context-Management Anti-Patterns

```text
Wrong: /compact because the next task is unrelated
Right: /clear or /new

Wrong: /clear before preserving important findings
Right: update the external checkpoint first

Wrong: save every conversation detail in copilot-instructions.md
Right: promote only stable repository rules

Wrong: paste a 20,000-line log into chat
Right: retain the log, extract decisive lines, and cite its location

Wrong: treat an internal compaction summary as the only source of truth
Right: keep evidence-backed project state outside the conversation
```

> Compact for continuity. Clear for separation. Save externally for durability.

---

## Slide 101 — What GitHub Copilot Memory Is

Copilot Memory is persistent context that Copilot can reuse across sessions when settings and policies allow it.

It is different from:

- the active conversation context;
- committed custom instruction files;
- an investigation checkpoint;
- an architectural decision record.

Copilot can create memories automatically while working. You can also ask it to remember something, but the model decides whether to store it and whether a stored memory is relevant later.

---

## Slide 102 — Two Memory Types

| Memory type | What it captures | Scope |
|---|---|---|
| **User-level preferences** | How you personally prefer Copilot to collaborate, present work, request approval, or follow reusable workflows | Your Copilot interactions across repositories |
| **Repository-level facts** | Codebase facts such as architecture, conventions, dependencies, build commands, test commands, and project-specific rules | One repository; shared with eligible collaborators |

Copilot chooses the type based on who the information applies to—not simply who said it.

---

## Slide 103 — Memory Does Not Have a Repository File Path

Copilot Memory is not stored in:

```text
.github/copilot-instructions.md
AGENTS.md
.github/skills/
```

Management locations:

```text
User preference:
GitHub personal settings → Copilot → Memory

Repository fact:
Repository → Settings → Copilot → Memory
```

Repository administrators and organization or enterprise policies can control whether repository facts are stored and used. Client-level settings can further restrict memory.

---

## Slide 104 — Real Workshop Repository Paths

Repository root:

```text
C:\Users\tysch\Documents\Copilot-Beyond-Autocomplete-Workshop
```

Useful evidence paths:

```text
.github/copilot-instructions.md
.github/instructions/checkout-domain.instructions.md
.github/instructions/migrations.instructions.md
demo/src/Store.Checkout/CheckoutCalculator.cs
demo/src/Store.Checkout.Persistence/CheckoutDbContext.cs
demo/src/Store.Checkout.Persistence/Migrations/20261029200000_InitialCheckout.cs
demo/scripts/Verify-Baseline.ps1
demo/scripts/Verify-Fixed.ps1
demo/scripts/Reset-Demo.ps1
```

A repository memory should cite current source evidence rather than restating an unsupported claim.

---

## Slide 105 — Real User-Level Preference Example

Prompt Copilot:

```text
Remember this as my user-level preference across repositories:
Explain the intended change before making multi-file edits, and ask for
my approval before generating or applying database migrations.
```

Why this is user-level:

- It describes how one person prefers to work.
- It applies across repositories.
- It is not a fact about the checkout codebase.
- It contains no sensitive information.

Do not phrase a personal preference as though it were a team-wide repository rule.

---

## Slide 106 — Real Repository-Level Fact Example

Prompt Copilot from the workshop repository:

```text
Create a repository-level memory for this repository:
The presentation baseline is verified by
`demo/scripts/Verify-Baseline.ps1`.
The corrected state is verified by
`demo/scripts/Verify-Fixed.ps1`, and
`demo/scripts/Reset-Demo.ps1` restores the intentional baseline.
Check these files on the current branch before using this fact.
```

Why this is repository-level:

- The paths exist only in this repository.
- The workflow is useful to any workshop contributor.
- The fact can be checked against current files.
- It should not follow the user into unrelated repositories.

Storage is not guaranteed merely because you requested it; verify in repository Memory settings.

---

## Slide 107 — Another Repository Fact: Persistence Layout

Possible prompt:

```text
Create a repository-level memory:
Checkout persistence uses EF Core with SQLite in
`demo/src/Store.Checkout.Persistence`.
The DbContext is `CheckoutDbContext.cs`, and migration history is under
`demo/src/Store.Checkout.Persistence/Migrations`.
Verify these citations against the current branch before using the fact.
```

Evidence:

- `Store.Checkout.Persistence.csproj` references `Microsoft.EntityFrameworkCore.Sqlite`.
- `CheckoutDbContext.cs` contains the EF Core model.
- `Migrations/20261029200000_InitialCheckout.cs` contains migration history.

This is descriptive codebase knowledge, not permission to generate or apply migrations.

---

## Slide 108 — What Should Not Become Memory

Do not store:

- secrets, tokens, credentials, or customer data;
- temporary debugging hypotheses;
- one-time task status;
- stale branch-specific details;
- unverified claims;
- information that belongs in an ADR or test;
- mandatory team rules that must apply consistently;
- duplicated copies of existing committed instructions.

Example that should **not** become memory:

```text
The current checkout failure is probably caused by rounding.
```

The repository evidence rejects that hypothesis.

---

## Slide 109 — Memory vs Instructions vs ADR vs Checkpoint

| Need | Correct home |
|---|---|
| Personal collaboration preference across repositories | User-level Copilot Memory |
| Useful fact learned about one repository | Repository-level Copilot Memory |
| Rule Copilot should consistently follow in this repository | `.github/copilot-instructions.md` |
| Rule for matching files only | `.github/instructions/*.instructions.md` |
| Architectural decision requiring durable rationale | `docs/adr/*.md` |
| Temporary investigation evidence and next action | `demo/investigations/*.md` |
| Behavior that must be enforced | Tests, analyzers, CI, permissions, or deployment policy |

GitHub recommends custom instructions rather than memory when guidance must be applied consistently.

---

## Slide 110 — Real Example: Do Not Duplicate the Shipping Rule

The rule already exists at:

```text
.github/instructions/checkout-domain.instructions.md
```

```markdown
- Loyalty discounts apply to merchandise only; shipping is never discountable.
```

Do not also ask Copilot to store an identical repository memory.

Why:

- The instruction is version-controlled.
- Its path scope is explicit.
- The team can review changes.
- Duplicate memory can drift or remain after the file changes.

Use memory for helpful learned facts, not as an invisible replacement for committed guidance.

---

## Slide 111 — Memory Best Practices

Do:

- State intended scope explicitly: “for me across repositories” or “for this repository.”
- Store durable, non-sensitive, verified information.
- Use exact repository paths and evidence.
- Review stored memories in GitHub settings.
- Delete stale or incorrect memories.
- Keep mandatory rules in version-controlled instructions.
- Start a fresh session after disabling memory if previous memories are already in context.

Do not:

- Assume a request guarantees storage.
- Assume a stored memory will always be used.
- Treat memory as permanent truth.
- Use memory as a security or policy boundary.
- Store facts that are easier and safer to infer from current code.

---

## Slide 112 — Retention, Reinforcement, and Staleness

Copilot Memory is intentionally not permanent:

- The default expiry period is 90 days without reinforcement.
- User-level preferences use the default retention period.
- Organizations and enterprises can configure repository-fact retention from 7 to 180 days.
- Repository facts are checked against cited files on the current branch before use.
- Retrieving a memory alone does not necessarily make it authoritative.

Treat memory as helpful, reviewable context—not as the source of truth.

---

## Slide 113 — Workshop Exercise: Classify the Knowledge

| Statement | Best location |
|---|---|
| “Across repositories, explain changes before applying them.” | User-level memory |
| “The workshop reset script restores the intentional baseline.” | Repository-level fact, with path citation |
| “Shipping is never discountable.” | Existing path-specific instruction |
| “Use xUnit for tests.” | Version-controlled test instruction |
| “We rejected a migration because no schema participates.” | Investigation checkpoint |
| “Uploaded files remain quarantined until scanning succeeds.” | ADR plus tests; concise instruction if broadly needed |
| “Never apply production migrations without approval.” | Instructions plus technical permissions and deployment policy |

> Memory helps Copilot remember. It does not replace version control, executable evidence, or governance.

## Sources

- GitHub Docs — Choosing a model in GitHub Copilot CLI: https://docs.github.com/en/copilot/how-tos/copilot-cli/use-copilot-cli/choose-a-model
- GitHub Docs — Copilot CLI command reference: https://docs.github.com/en/copilot/reference/copilot-cli-reference/cli-command-reference
- VS Code Docs — AI language models in VS Code: https://code.visualstudio.com/docs/agent-customization/language-models
- GitHub Docs — Using GitHub Copilot CLI and managing context: https://docs.github.com/en/copilot/how-tos/copilot-cli/use-copilot-cli/overview
- GitHub Docs — Adding agent skills for GitHub Copilot CLI: https://docs.github.com/en/copilot/how-tos/copilot-cli/customize-copilot/add-skills
- GitHub Docs — About GitHub Copilot Memory: https://docs.github.com/en/copilot/concepts/prompting-and-context/copilot-memory
- GitHub Docs — Manually creating memories: https://docs.github.com/en/copilot/how-tos/use-copilot-agents/copilot-memory/create-memories
- GitHub Docs — Managing Copilot Memory for yourself: https://docs.github.com/en/copilot/how-tos/use-copilot-agents/copilot-memory/manage-for-yourself
