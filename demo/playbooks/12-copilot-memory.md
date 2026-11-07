# Demo 12 - Copilot Memory (User Preferences and Repository Facts)

## Fixture (already committed)

```text
demo\scenarios\copilot-memory-real-project.md
demo\src\Store.Checkout.Persistence\CheckoutDbContext.cs
demo\src\Store.Checkout.Persistence\Migrations\20261029200000_InitialCheckout.cs
```

## Prerequisite

Use a Copilot session with memory enabled, opened at the repository root.
Review existing memories first so the demonstration does not create duplicates.

## Steps to show

1. Open the DbContext and migration files above to verify the repository paths.
2. Run the user-level preference prompt below. Highlight that its scope is explicit:
   these collaboration preferences apply across all repositories.
3. Run the repository-level fact prompt below. Highlight that repository layout
   belongs to this repository, not to the user's other projects.
4. Inspect the response and the stored memories in the appropriate Memory settings.
   A request to remember something is not proof that it was stored.
5. Contrast these examples with the existing scoped migration instructions:
   memory about file locations does not authorize schema changes or database updates.

## Prompt (user-level preferences)

```text
Remember this as my user-level preference across all repositories:

Prefer the smallest change that fixes the problem rather than an
unrelated refactor.

Explain the intended change before making multi-file edits.

Ask for my approval before generating or applying database migrations.

Run the relevant tests and summarize their results before proposing
a commit.
```

## Prompt (repository-level fact)

```text
Create a repository-level memory:

EF Core migration history for this repository is stored under
`demo\src\Store.Checkout.Persistence\Migrations`.

The DbContext is:
`demo\src\Store.Checkout.Persistence\CheckoutDbContext.cs`.

Verify these paths before storing or using this memory.
```

## Expected outcome

The assistant distinguishes user preferences from repository facts and verifies
the repository paths before storing them. If equivalent memories already exist,
it reinforces them rather than creating duplicates. If storage is unavailable,
it reports that limitation instead of claiming success.

No production files are edited, migrations generated or applied, tests run, or
commits created by these memory-only prompts. The testing preference governs
future coding work; it does not request a test run for this demonstration.

Memory is not an enforcement mechanism or a guarantee of future recall. Keep
contributor-wide business rules and migration safety rules in version-controlled
instructions.

## Say this

> "My collaboration preferences follow me across repositories. The DbContext
> location belongs only to this project. Neither memory gives permission to
> change a database."

## Reset

No source reset is needed. If the demo created temporary memories, delete only
those entries through the appropriate settings:

- User memories: <https://github.com/settings/copilot/memory>
- Repository memories: repository Settings > Copilot > Memory

Do not delete pre-existing preferences or repository facts as part of cleanup.
