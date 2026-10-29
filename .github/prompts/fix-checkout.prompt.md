---
name: fix-checkout
description: Plan, implement, and verify the smallest checkout-domain fix.
agent: agent
tools: [read, search, edit, execute]
---

The loyalty checkout example is wrong for merchandise $100, shipping $10, and a 10% loyalty discount.

Work in this order:

1. Read the repository and path-specific instructions that apply.
2. Run the focused checkout tests and report the exact failing assertion.
3. Explain the business invariant and identify the authoritative calculation.
4. Propose a minimal plan before editing.
5. Change only production code unless a missing contract test is required.
6. Run:
   - `dotnet test demo/tests/Store.Checkout.Tests/Store.Checkout.Tests.csproj`
   - `dotnet test CopilotWorkshop.slnx`
   - `dotnet build CopilotWorkshop.slnx`
   - `git diff --stat` and `git diff`
7. Finish with changed files, the rule restored, and verification evidence.

Do not change tests to hide the failure. Do not duplicate the calculation in the console app.
