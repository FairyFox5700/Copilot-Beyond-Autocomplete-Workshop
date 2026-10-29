# Participant Worksheet

## Scenario

A free-shipping promotion may reduce shipping to zero, but a loyalty discount must still apply only to merchandise. Every change must include a focused test and must not duplicate pricing logic in the console application.

## Your four-minute task

### 1. Repository-wide instruction

Write one durable rule that applies broadly:

```text

```

### 2. Checkout-domain instruction

Write one rule that belongs only with checkout-domain files:

```text

```

### 3. Reusable prompt workflow

Write six steps:

```text
1.
2.
3.
4.
5.
6.
```

## Self-check

- [ ] The repository-wide rule is useful beyond this single task.
- [ ] The checkout rule is scoped to the domain path.
- [ ] The workflow begins with evidence.
- [ ] The workflow names the authoritative layer.
- [ ] The workflow requires focused and full verification.
- [ ] The workflow includes diff/scope review.

---

# Answer key

## Repository-wide

```text
- Keep business rules in the domain project; UI and console projects consume domain results and do not recalculate them.
```

## Checkout-domain

```text
- A free-shipping promotion may reduce shipping to zero, but loyalty discounts are calculated only from merchandise subtotal.
```

## Reusable workflow

```text
1. Read applicable repository and checkout instructions.
2. Add or identify a focused failing boundary test.
3. Run it and capture the exact failure.
4. Change only the authoritative domain calculation.
5. Run focused tests, full tests, build, and the visible consumer.
6. Review the diff and report evidence plus risks.
```

## Why this answer works

The durable architectural rule is repository-wide. The promotion and discount invariant is checkout-specific. The prompt owns the sequence for a particular class of work. Verification is executable, and the diff review checks scope rather than trusting a green test alone.
