# Demo 5 — Scoped Instructions Stay Out of Context (Migration Example, GitHub Copilot CLI)

## Fixture (already committed)

```text
.github/instructions/migrations.instructions.md
applyTo: "demo/src/Store.Checkout.Persistence/Migrations/**/*.cs"
```

## Prerequisite

GitHub Copilot CLI installed locally. Confirm once before presenting:

```powershell
copilot --version
```

Run everything below from the repository root.

## Steps to show

1. Open `.github/instructions/migrations.instructions.md` in a terminal pager
   (`cat .github/instructions/migrations.instructions.md` or `code` if available) and
   point at the `applyTo` glob in the frontmatter — this is the mechanism, not a
   convention Copilot has to guess at.
2. Run `copilot instruction list` — this is a static inventory of every instruction
   source the repository has, regardless of file. Point out that all four files
   (`copilot-instructions.md`, `checkout-domain`, `migrations`, `tests`) are listed here
   because this command shows what's *discoverable*, not what's *active* for a given
   file — that distinction is the whole point of this demo.
3. Run the outside-glob prompt below. Point at the result: `migrations.instructions.md`
   is absent.
4. Run the inside-glob prompt below. Point at the result: `migrations.instructions.md`
   is now present.
5. Compare the two outputs side by side.

## Command (step 2, static discovery)

```powershell
copilot instruction list
```

## Expected output (step 2)

```text
Repository instructions:
  • .github/copilot-instructions.md
      from .github/copilot-instructions.md
  • checkout-domain.instructions.md - Business invariants for checkout calculations.
      from .github\instructions\checkout-domain.instructions.md
  • migrations.instructions.md - Safety rules for generated and reviewed database migrations.
      from .github\instructions\migrations.instructions.md
  • tests.instructions.md - Deterministic xUnit conventions for checkout tests.
      from .github\instructions\tests.instructions.md
```

## Prompt (step 3, outside glob)

```powershell
copilot -p "Look at demo/src/Store.Checkout/CheckoutCalculator.cs. Which instruction files from this repository apply to this file, and why? List only the applicable ones by path." -s
```

## Expected output (step 3)

```text
- .github\copilot-instructions.md
- .github\instructions\checkout-domain.instructions.md
```

## Prompt (step 4, inside glob)

```powershell
copilot -p "Look at demo/src/Store.Checkout.Persistence/Migrations/20261029200000_InitialCheckout.cs. Which instruction files from this repository apply to this file, and why? List only the applicable ones by path." -s
```

## Expected output (step 4)

```text
- .github\copilot-instructions.md
- .github\instructions\migrations.instructions.md
```

Note: the migration filename above is the one currently committed in this repo
(`demo/src/Store.Checkout.Persistence/Migrations/`). If a new migration has been added
since, substitute the current filename — any file under that folder matches the glob.

## Say this

> "Do I need migration-naming and safety rules every time I touch checkout pricing logic? No — this is why path-scoped instructions exist instead of one global file with everything in it. The static list shows everything that exists in the repository; the per-file prompt shows what actually applies to the file you're working on."

## Reset

Nothing to reset — no files are changed during this demo, all commands are read-only.
