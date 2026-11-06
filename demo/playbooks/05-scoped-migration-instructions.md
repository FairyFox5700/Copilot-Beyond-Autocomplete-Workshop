# Demo 5 — Scoped Instructions Stay Out of Context (Migration Example)

## Fixture (already committed)

```text
.github/instructions/migrations.instructions.md
applyTo: "demo/src/Store.Checkout.Persistence/Migrations/**/*.cs"
```

## Steps to show

1. Open `.github/instructions/migrations.instructions.md` and point at the `applyTo` glob at the top — this is the mechanism, not a convention Copilot has to guess at.
2. Open a file that is **not** covered by the glob, e.g. `demo/src/Store.Checkout/CheckoutCalculator.cs`.
3. In Copilot Chat, run: `/instructions` — show the active instruction list. Migration rules are **not** in it.
4. Ask a throwaway question in this context, e.g. "What should I keep in mind editing this file?" — the answer should not mention migration safety rules.
5. Open a file that **is** covered by the glob, e.g. any file under `demo/src/Store.Checkout.Persistence/Migrations/`.
6. Run `/instructions` again — now the migration rules appear in the active list.
7. Ask the same style of question again and show the migration-safety rules now influence the answer.

## Prompt (step 4, outside glob)

```text
What should I keep in mind editing this file?
```

## Prompt (step 7, inside glob)

```text
What should I keep in mind editing this file?
```

## Expected outcome

```text
Outside the Migrations folder: /instructions does not list migrations.instructions.md,
and the response does not reference migration safety rules.

Inside the Migrations folder: /instructions lists migrations.instructions.md, and the
response references at least one of:
- treating existing migrations as immutable history;
- reviewing both Up() and Down();
- calling out destructive operations;
- never inferring permission to apply a migration from permission to edit the file.
```

## Say this

> "Do I need migration-naming and safety rules every time I touch checkout pricing logic? No — this is why path-scoped instructions exist instead of one global file with everything in it."

## Reset

Nothing to reset — no files are changed during this demo.
