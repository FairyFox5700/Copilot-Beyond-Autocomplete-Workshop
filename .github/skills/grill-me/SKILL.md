---
name: grill-me
description: Interview the user before implementing a non-trivial or ambiguous feature. Use when requirements, security boundaries, failure behavior, or acceptance criteria are not yet explicit.
---

# Grill Me Before Implementation

Do not write production code when this skill starts. First turn the request into an approved, testable implementation contract.

## Interview workflow

1. Inspect the repository before asking questions. Identify the likely architecture, authoritative layer, existing patterns, relevant tests, and applicable instruction files.
2. Restate the requested outcome in one sentence and identify the first material ambiguity.
3. Ask one decision-changing question at a time. Briefly explain why the answer affects implementation, security, compatibility, operations, or testing.
4. Challenge vague answers. Do not accept phrases such as "any file," "normal users," "handle errors," "secure it," or "add tests" without measurable meaning.
5. Prefer concrete choices when they help the user decide, but do not choose a product or policy without approval.
6. Continue until every material decision is either resolved or explicitly recorded as an accepted assumption.

## Areas to cover when relevant

- User outcome, actors, and out-of-scope behavior
- API contract, inputs, outputs, status codes, and compatibility
- Authentication, authorization, tenant boundaries, and ownership
- Validation based on trusted content rather than client-supplied labels
- Size, rate, concurrency, and performance limits
- Storage location, naming, retention, encryption, and deletion
- Duplicate requests, idempotency, and retry behavior
- Partial failures, rollback, recovery, and external-service outages
- Malware scanning, quarantine, secrets, and sensitive data
- Logging, metrics, tracing, audit events, and alerting
- Success, failure, boundary, abuse, and cancellation tests
- Rollout, migration, backward compatibility, and feature flags

## Required decision summary

After the interview, produce:

1. **Decision log** — each question, approved answer, and implementation consequence.
2. **Unresolved assumptions** — anything still unclear, with the risk of proceeding.
3. **Smallest implementation** — authoritative layer, files likely to change, and files that must not change.
4. **Test plan** — success, failure, boundary, security, and recovery cases.
5. **Verification plan** — exact focused tests, broader tests, build, runtime check, and diff review.

End with:

> I have not written code. Approve or revise this contract before implementation.

## After approval

Only after explicit approval:

1. Follow repository-wide and path-specific instructions.
2. Implement the smallest cohesive change.
3. Add or update tests for the approved contract.
4. Run the verification plan.
5. Review the final diff for unrelated changes.
6. Report commands executed, results, remaining risks, and anything not verified.

Never treat an instruction or interview answer as a security boundary. Use permissions, validation, tests, CI, and deployment controls for enforcement.
