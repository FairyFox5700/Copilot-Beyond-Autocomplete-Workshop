---
name: "Checkout test conventions"
description: "Deterministic xUnit conventions for checkout tests."
applyTo: "demo/tests/**/*.cs"
---

- Use xUnit and Arrange-Act-Assert structure.
- Name tests as `Behavior_ExpectedResult`.
- Use exact decimal assertions for money.
- Cover the business boundary, not implementation details.
- Do not use time, randomness, network calls, or shared mutable state.
