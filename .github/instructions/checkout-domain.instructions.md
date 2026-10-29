---
name: "Checkout domain rules"
description: "Business invariants for checkout calculations."
applyTo: "demo/src/Store.Checkout/**/*.cs"
---

- Loyalty discounts apply to merchandise only; shipping is never discountable.
- Keep one authoritative calculation in the domain project.
- Return an explicit breakdown so API and UI consumers do not recalculate totals.
- Reject negative money values and discount rates outside zero through one.
