# Presenter Scenario — Context and Skill Audit

## Purpose

Show that usage, context allocation, instruction discovery, skill inventory, and session artifacts are different concerns with different commands.

Do not present fixed token numbers: results vary by model, account, CLI version, enabled tools, and prior conversation.

## Part 1 — Start with separate diagnostics

In a GitHub Copilot CLI interactive session at the workshop repository root, run:

```text
/usage
```

Discuss:

- session duration;
- models used;
- token totals by model;
- edited lines;
- AI credits when shown for the account.

Then run:

```text
/context
```

Discuss:

- system and tool allocation;
- message-history allocation;
- free space;
- reserved buffer.

Teaching point:

> `/usage` explains consumption across the session. `/context` explains the current window allocation.

## Part 2 — Explain what may be influencing the session

Run:

```text
/instructions
/skills list
/skills info grill-me
/skills info evidence-checkpoint
/session
/session plan
/session files
```

Expected workshop inventory:

- repository-wide Copilot instructions;
- checkout-domain instructions;
- test instructions;
- migration instructions;
- `grill-me` project skill;
- `evidence-checkpoint` project skill.

Do not claim every discovered skill body is already injected. Copilot injects a skill's instructions when the skill is chosen or invoked.

## Part 3 — Compare two tasks

### Task A: Known checkout defect

Evidence already available:

- one explicit business invariant;
- one failing focused test;
- one authoritative method;
- deterministic red/green scripts.

Decision:

```text
Do not invoke grill-me or evidence-checkpoint merely because they exist.
```

### Task B: File-upload feature

The task includes authorization, trusted content validation, duplicate handling, object storage, compensation, quarantine, and asynchronous malware scanning.

Decision:

```text
Use grill-me for the requirements phase.
Use evidence-checkpoint for the long investigation and model handoff.
Do not stack unrelated review or migration skills before they are needed.
```

## Part 4 — Demonstrate skill hygiene

Inspect a skill before using it:

```text
/skills info grill-me
```

Questions:

1. Is its description narrow enough?
2. Does it duplicate repository instructions?
3. Is its workflow relevant to this phase?
4. Does it load optional detail only when needed?
5. Does another enabled skill prescribe the same workflow?

Use `/skills` to enable or disable skills interactively. The terminal command equivalents include:

```text
copilot skill list
copilot skill disable SKILL-NAME
copilot skill enable SKILL-NAME
```

Disabling a skill does not erase content already present in conversation history. For a truly clean task boundary, create a checkpoint and start a focused session.

## Part 5 — Compact with evidence

Before compaction, update:

```text
demo/investigations/file-upload-model-handoff.md
```

Then run:

```text
/compact focus on the verified upload decisions, evidence,
unresolved risks, and approved next action
```

Inspect the summary:

```text
/session checkpoints
/session checkpoints NUMBER
```

Compare it with the repository handoff. If the summary omits a critical decision, the durable handoff remains authoritative.

## Final takeaway

```text
Usage metrics      → /usage
Window allocation  → /context
Instruction files  → /instructions
Skill inventory    → /skills list and /skills info
Session artifacts  → /session commands
History reduction  → /compact
Durable truth      → repository checkpoint with evidence
```
