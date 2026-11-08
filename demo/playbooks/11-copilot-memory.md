# Demo 11 - Copilot Memory

## [Enable user-level preferences](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/copilot-memory/manage-for-yourself#disabling-or-enabling-copilot-memory)

## [Enable repository-level facts](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/copilot-memory/manage-as-administrator#managing-copilot-memory-for-repositories)

## Enable memory in the Copilot CLI session

```text
/memory on
```

## User preferences

```text
Remember these user-level preferences across all repositories:
- Prefer the smallest fix over unrelated refactoring.
- Explain the intended change before multi-file edits.
- Ask for approval before generating or applying database migrations.
- Run relevant tests and summarize results before proposing a commit.
Avoid duplicate memories. Report if memory storage is unavailable.
```

## Repository facts

```text
Verify and remember these repository-level paths:
- EF Core migrations: demo\src\Store.Checkout.Persistence\Migrations
- DbContext: demo\src\Store.Checkout.Persistence\CheckoutDbContext.cs
Avoid duplicate memories. Report if memory storage is unavailable.
Do not modify files or generate or apply migrations.
```

## Verify storage

```text
Confirm which memories were actually stored and their scopes.
Report any storage or verification limitations without claiming success.
```

## Remove only demo-created memories in Memory settings
