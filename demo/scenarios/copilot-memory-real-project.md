# Presenter Scenario — Copilot Memory in the Real Workshop Project

## Repository

Absolute path:

```text
C:\Users\tysch\Documents\Copilot-Beyond-Autocomplete-Workshop
```

Copilot Memory is not a local file in this directory. The repository paths below are evidence that a repository-level memory can cite.

## Step 1 — Inspect the evidence first

Open:

```text
.github/copilot-instructions.md
.github/instructions/checkout-domain.instructions.md
.github/instructions/migrations.instructions.md
demo/src/Store.Checkout.Persistence/Store.Checkout.Persistence.csproj
demo/src/Store.Checkout.Persistence/CheckoutDbContext.cs
demo/src/Store.Checkout.Persistence/Migrations/20261029200000_InitialCheckout.cs
demo/scripts/Verify-Baseline.ps1
demo/scripts/Verify-Fixed.ps1
demo/scripts/Reset-Demo.ps1
```

Do not create a memory from an assumption. Confirm the files and behavior first.

## Step 2 — User-level preference example

Prompt:

```text
Remember this as my user-level preference across repositories:
Explain the intended change before making multi-file edits, and ask for
my approval before generating or applying database migrations.
```

Why this belongs at user level:

- it describes one person's collaboration style;
- it is useful in multiple repositories;
- it is not a fact about the checkout architecture;
- it contains no secret or customer information.

Review or delete it in personal GitHub Copilot Memory settings. Storage is not guaranteed merely because the prompt requested it.

## Step 3 — Repository-level fact example

Prompt from the workshop repository:

```text
Create a repository-level memory for this repository:
The presentation baseline is verified by
`demo/scripts/Verify-Baseline.ps1`.
The corrected state is verified by
`demo/scripts/Verify-Fixed.ps1`, and
`demo/scripts/Reset-Demo.ps1` restores the intentional baseline.
Check these files on the current branch before using this fact.
```

Why this belongs at repository level:

- the paths exist only here;
- collaborators can use the same presentation workflow;
- the fact can be checked against version-controlled evidence;
- it must not follow the user into unrelated repositories.

Review or delete it under:

```text
Repository → Settings → Copilot → Memory
```

## Step 4 — Repository fact about persistence

Prompt:

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
- `CheckoutDbContext.cs` defines the model.
- `Migrations/20261029200000_InitialCheckout.cs` defines migration history.

This memory describes layout. It does not authorize Copilot to generate or apply migrations.

## Step 5 — Example that should remain an instruction

Existing path:

```text
.github/instructions/checkout-domain.instructions.md
```

Existing rule:

```text
Loyalty discounts apply to merchandise only; shipping is never discountable.
```

Do not duplicate this as memory. It is already version-controlled, path-scoped, reviewable, and expected to apply consistently.

## Step 6 — Example that belongs in the investigation checkpoint

Existing path:

```text
demo/investigations/checkout-discount-checkpoint.md
```

Rejected hypothesis:

```text
Currency rounding causes the $1.00 difference.
```

This is temporary investigation history, not durable repository memory.

## Best-practice checklist

Before asking Copilot to remember something:

1. Is it true and verified?
2. Is the scope explicitly personal or repository-specific?
3. Is it non-sensitive?
4. Will it remain useful beyond this task?
5. Does it already belong in instructions, an ADR, a test, or a checkpoint?
6. Can a repository fact cite current files?
7. Can a collaborator review or delete it later?

## Important limitations

- Copilot decides whether to store the requested memory.
- A stored memory is not guaranteed to be used every time.
- Repository facts are checked against cited current-branch evidence.
- Memories expire without reinforcement; the default is 90 days.
- Memory is not an enforcement mechanism.
- If memory is disabled after it entered a conversation, start a new session to continue without that existing context.
