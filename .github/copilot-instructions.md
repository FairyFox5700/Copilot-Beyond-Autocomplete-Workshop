# Store checkout repository instructions

- This repository uses .NET 8 and C#.
- Keep checkout business rules in `demo/src/Store.Checkout`; the console app is only a consumer.
- Preserve public APIs unless the request explicitly requires a breaking change.
- Use `decimal` for money and round currency with `MidpointRounding.AwayFromZero`.
- Make the smallest change that satisfies the stated contract.
- Never weaken or delete a failing test merely to make the suite pass.
- Before claiming completion, run the focused tests, the full solution tests, and the solution build. Report the commands and results.

## C# conventions

- `<Nullable>enable</Nullable>` is on for every project; do not introduce `#pragma warning disable CS8xxx` or `!` null-forgiving operators to silence warnings — fix the nullability instead.
- Favor `sealed` classes and records unless a type is explicitly designed for inheritance.
- Use file-scoped namespaces (`namespace Store.Checkout;`), not block-scoped ones.
- Prefer primary constructors (e.g. `CheckoutDbContext(DbContextOptions<CheckoutDbContext> options)`) over constructor bodies that only assign fields.
- Validate arguments with the `ArgumentOutOfRangeException.ThrowIfNegative` / `ArgumentNullException.ThrowIfNull` static guard-clause helpers instead of hand-written `if` + `throw` blocks, except where the business rule is domain-specific (see `CheckoutCalculator.Calculate`'s discount-rate range check).
- Model domain outputs as `sealed record` types (e.g. `CheckoutBreakdown`) rather than mutable classes or tuples when the value is immutable and compared by value.
- Keep EF Core entities (`demo/src/Store.Checkout.Persistence`) as plain mutable classes with `required` properties for mandatory fields; configure columns explicitly in `OnModelCreating` (e.g. `HasColumnType("decimal(18,2)")` for money, `HasMaxLength` for strings) rather than relying on EF conventions.
- Async methods must be suffixed `Async` and accept/pass a `CancellationToken` when they do I/O (database, file, network).
- Avoid LINQ chains that re-enumerate (`.Count()` after `.Where()` then `.ToList()` again); materialize once with `.ToList()`/`.ToArray()` when a sequence is used more than once.
- Do not use exceptions for expected control flow in hot paths (e.g. normal validation failures at the API boundary); reserve thrown exceptions for truly exceptional, programmer-error conditions as this codebase already does in `CheckoutCalculator`.
- Comments should explain *why*, not *what* — the one existing exception in this repo (`// Intentional workshop bug: shipping must never be discounted.`) marks a deliberate defect, not a style template; do not add narrative comments restating the code.
