# Presenter Scenario — Compact, Clear, or Save Externally?

## Principle

```text
Compact        = continue the same task with compressed conversation history
Clear or new   = begin a different task without the previous active conversation
External state = preserve important knowledge beyond the active conversation
```

Do not use these as interchangeable cleanup techniques.

## Scenario 1 — Same checkout investigation, crowded context

The agent has inspected the calculator, tests, console application, and persistence project. The task is still to fix the loyalty discount, but message history contains repeated searches and long test output.

Before compaction, update:

```text
demo/investigations/checkout-discount-checkpoint.md
```

Then run:

```text
/compact focus on the confirmed checkout defect, rejected hypotheses,
verification commands, and next action
```

Inspect the summary:

```text
/session checkpoints
/session checkpoints NUMBER
```

Decision: **compact**, because the task and evidence chain continue.

## Scenario 2 — Checkout defect finished; migration review begins

The discount fix is complete and verified. The next task is to review an EF Core migration for destructive operations.

Preserve the final verification result, then run:

```text
/clear Review the checkout migration for destructive operations.
Do not modify or apply migrations.
```

Decision: **clear**, because the new task does not need the defect conversation.

## Scenario 3 — File-upload design spans several sessions

The requirements interview established authorization, content validation, duplicate handling, private storage, compensation, quarantine, and asynchronous malware scanning.

Preserve the decisions in:

```text
demo/investigations/file-upload-model-handoff.md
```

A future session starts with:

```text
Read demo/investigations/file-upload-model-handoff.md.
Verify its cited repository evidence and continue from the stated next action.
Do not reopen approved decisions without contradictory evidence.
```

Decision: **external state**, because the knowledge must survive sessions, model changes, and compaction.

## Scenario 4 — Huge command output

A test or build emits thousands of lines.

Do not paste the entire output into the active context. Retain the complete log, then record:

- command executed;
- exit code;
- decisive error lines;
- log location;
- conclusion and confidence.

Decision: **selective evidence**, not compaction after flooding the context.

## Scenario 5 — Stable architecture decision

The team decides that uploaded files remain private and quarantined until malware scanning succeeds.

Store the architectural decision in a maintained ADR. Keep detailed investigation evidence in the handoff. Add a concise repository instruction only if the rule applies broadly to future coding tasks.

Decision: **promote stable knowledge**, rather than copying the whole conversation into `.github/copilot-instructions.md`.

## Audience exercise

Classify each action:

| Situation | Answer |
|---|---|
| Same debugging task, long history | Checkpoint + compact |
| Unrelated task begins | Clear or new conversation |
| Decision must survive tomorrow | External state |
| Massive log needs one error | Selective extraction |
| Separate subsystem research | Bounded read-only subagent |

## Final line

> Compact for continuity. Clear for separation. Save externally for durability.
