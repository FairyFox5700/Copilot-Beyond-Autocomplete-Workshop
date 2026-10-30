---
name: evidence-checkpoint
description: Preserve evidence and decisions during long, multi-file investigations. Use when work may exceed one context window, involve subagents, or require a reliable handoff.
---

# Evidence Checkpoint

Treat conversation context as temporary working memory. Keep durable investigation state in a repository scratchpad so another agent or a fresh session can continue without reconstructing the work.

## Start

1. Restate the investigation question and definition of done.
2. Create or update a scratchpad using `templates/investigation-checkpoint.md`.
3. Record the current repository state, relevant branch or commit, and commands already executed.
4. Split broad investigations into independent evidence questions before delegating work.

## Evidence rules

- Record claims with file paths, line ranges, command output, test names, or source links.
- Label each item as `confirmed`, `hypothesis`, `decision`, `rejected`, or `unverified`.
- Keep raw logs outside the conversation when large; record their location and the decisive excerpt.
- Do not treat a subagent summary as verified evidence. Check the cited file, command, or test before promoting the claim to `confirmed`.
- When two agents disagree, prefer executable evidence and authoritative source code over confidence or majority opinion.

## Subagent contract

Give each subagent one non-overlapping question and require this output:

1. conclusion;
2. evidence with exact paths and line ranges;
3. commands executed and results;
4. unresolved questions;
5. recommended next action;
6. files changed, or `none`.

Use read-only subagents during investigation unless isolated worktrees are available and implementation was explicitly approved.

## Checkpoint moments

Update the scratchpad:

- after discovering a material fact;
- after a decision changes scope;
- before switching from investigation to implementation;
- before context compaction, session reset, or agent handoff;
- after tests or commands change the confidence of a claim;
- before reporting completion.

## Required handoff

Produce a compact handoff containing:

- goal and current status;
- confirmed findings with evidence;
- decisions and their rationale;
- rejected hypotheses;
- unresolved questions and risks;
- commands executed and exact results;
- current changed-file list;
- the single next recommended action.

A handoff must be sufficient for a fresh agent to continue after reading only the scratchpad and cited files.

## Context-management decision

Use the right mechanism before continuing:

- **Same task, crowded conversation:** update the scratchpad, then use `/compact` with focus instructions. Review the generated `/session checkpoints` summary afterward.
- **Different or unrelated task:** preserve anything valuable, then start a fresh conversation with `/clear` or `/new`.
- **Knowledge must survive sessions:** store it in the repository scratchpad, an ADR, tests, or another maintained external artifact.

Do not treat a compaction summary as the only evidence record, and do not place temporary investigation details into repository-wide instructions.

## Completion

Before closing the investigation:

1. Verify all completion claims against commands or source evidence.
2. Remove obsolete hypotheses or mark them rejected rather than leaving contradictions.
3. Ensure the scratchpad names the final decision and verification result.
4. Review the diff for accidental scratch files, logs, or unrelated code changes.
