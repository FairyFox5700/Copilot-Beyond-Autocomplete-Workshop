# Store checkout repository instructions

- This repository uses .NET 8 and C#.
- Keep checkout business rules in `demo/src/Store.Checkout`; the console app is only a consumer.
- Preserve public APIs unless the request explicitly requires a breaking change.
- Use `decimal` for money and round currency with `MidpointRounding.AwayFromZero`.
- Make the smallest change that satisfies the stated contract.
- Never weaken or delete a failing test merely to make the suite pass.
- Before claiming completion, run the focused tests, the full solution tests, and the solution build. Report the commands and results.
