# Presenter Scenario — Prompt Queue and Recall

## Purpose

Demonstrate the GitHub Copilot CLI equivalent of stashing a draft prompt while another turn is running.

GitHub Copilot CLI does not document Claude Code's `Ctrl+S` stash behavior. In a local Copilot CLI session, use queue, recall, and external-editor controls instead.

## Setup

Start the file-upload investigation:

```text
Use the grill-me and evidence-checkpoint skills.
Inspect the repository for upload, storage, authorization, and
background-processing patterns. Do not edit files.
```

While Copilot is still working, type this next prompt:

```text
Using the investigation results, propose the smallest provider-neutral
implementation and test plan. Do not write code until I approve.
```

## Queue the draft

Press:

```text
Ctrl+Q
```

`Ctrl+Enter` is an alternative. The entry should appear as pending.

## Change the draft after new evidence

Assume the investigation establishes that uploaded content must remain quarantined during asynchronous malware scanning.

With an empty prompt box, press:

```text
↑
```

Recall the most recently queued prompt and revise it to:

```text
Using the verified investigation checkpoint, propose the smallest
provider-neutral implementation and test plan.

Include quarantine and asynchronous scan states.
Do not expose file content before a clean scan.
Do not write code until I approve.
```

Queue it again with `Ctrl+Q`, or wait and submit it normally after reviewing the current result.

## Change model without losing a typed draft

If the draft is still in the prompt box and planning needs a different model:

```text
Ctrl+X then /
```

Run `/model`, select an available model and reasoning level, then return to the draft.

## Long prompt option

Use:

```text
Ctrl+G
```

Edit the draft in the configured external editor, save it, and return to the Copilot CLI.

## Safety note

Do not queue implementation before the plan is approved. Queueing controls execution order; it does not replace a checkpoint, decision log, or human approval.

## Expected teaching outcome

Participants can distinguish:

- **queue** — schedule a ready prompt;
- **recall** — bring a queued prompt back for editing;
- **external editor** — safely compose a long prompt;
- **checkpoint** — preserve durable evidence and decisions;
- **model switch** — change execution capability without rebuilding the draft.
