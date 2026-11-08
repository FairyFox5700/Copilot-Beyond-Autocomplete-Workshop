# Demo 6 - Evidence Checkpoint + Live Model Switching

## Select Claude Haiku 5.5 with low reasoning effort

```text
/model
```

## Preserve Demo 5's evidence

```text
Use the evidence-checkpoint skill to preserve the confirmed checkout findings in
demo\investigations\checkout-discount-checkpoint.md.
Include evidence, decisions, rejected hypotheses, test results, and the next action.
Do not repeat the fleet investigation or change any other files.
```

## Select GPT-6.1 Sol with high reasoning effort

```text
/model
```

## Continue from the checkpoint

```text
Read only demo\investigations\checkout-discount-checkpoint.md.
Using only its evidence, state the exact one-line fix and file path.
Do not re-investigate, reread source files, or apply the change.
```

## Continue to Demo 7 in Plan mode
