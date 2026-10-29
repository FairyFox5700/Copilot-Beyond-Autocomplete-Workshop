# Slide Ideas — Grill Me Before You Build It

## Slide 32 — Give Copilot a Deliberately Vague Feature

```text
Build an API endpoint that lets users upload files.
```

- Familiar C# feature
- Sounds small
- Hides product, security, storage, and operational decisions
- Do not let Copilot implement immediately

**Workshop action:** invoke the `grill-me` project skill and paste the request.

---

## Slide 33 — Let Copilot Interview You

**Copilot:**

> What types of files should the endpoint accept, and what is the maximum file size? “Any file” is not sufficient because it affects validation, storage, and security.

**You:**

> PDF and PNG files, up to 10 MB.

**Copilot:**

> How will the application verify that the content is actually a PDF or PNG rather than trusting a client-supplied MIME type or filename extension?

**Teaching point:** every answer changes the implementation and tests.

---

## Slide 34 — Questions That Change the Architecture

Continue the interview into:

- Who may upload files?
- Who may read or delete them?
- How is tenant ownership enforced?
- What counts as a duplicate upload?
- Where are files and metadata stored?
- What happens when storage succeeds but metadata persistence fails?
- Is malware scanning synchronous, asynchronous, or absent?
- Are files quarantined before scanning completes?
- What status codes represent accepted, rejected, duplicate, and failed uploads?
- Which success, boundary, failure, and abuse cases require tests?

---

## Slide 35 — Example Decision Chain

```text
PDF and PNG only
        ↓
Content signatures must be inspected
        ↓
10 MB maximum
        ↓
Request-size and streaming limits
        ↓
Authenticated owners only
        ↓
Authorization and tenant tests
        ↓
Malware scan before availability
        ↓
Quarantine state and asynchronous workflow
```

**Teaching point:** requirements discovery is architecture work.

---

## Slide 36 — Prompt vs Instructions vs Skill

### A. One-off prompt

```text
Interview me before implementing this feature.
```

- Fast to try
- Easy to forget
- Quality varies between tasks

### B. Repository instructions

```text
For non-trivial features, clarify security, failure behavior,
and acceptance criteria before implementation.
```

- Broad, durable team rule
- Loaded with many repository tasks
- Should remain concise

### C. Reusable skill

```text
.github/skills/grill-me/SKILL.md
```

- Named multi-step workflow
- Loaded when relevant
- Carries its own interview, decision-log, planning, and verification process

---

## Slide 37 — What the Grill-Me Skill Packages

- Repository inspection before questions
- One decision-changing question at a time
- Rejection of vague requirements
- Security and authorization questions
- Failure and recovery questions
- Test and observability questions
- Decision log
- Unresolved-assumption register
- Smallest implementation proposal
- Approval gate before coding
- Verification after implementation

**Key distinction:** a skill is not merely a longer prompt; it packages repeatable behavior.

---

## Slide 38 — Run the Workshop Demo

Start with:

```text
Use the grill-me skill.

Build an API endpoint that lets users upload files.
```

Presenter answers:

```text
PDF and PNG files, up to 10 MB.
```

Continue until Copilot covers:

1. content verification;
2. authorization and ownership;
3. duplicate handling;
4. storage and partial failures;
5. malware scanning and quarantine;
6. test cases.

---

## Slide 39 — Stop Before Code

After the interview, submit:

```text
Now summarize the decisions we made.

Identify assumptions I failed to resolve.
Propose the smallest implementation and a test plan.
Name the files that should and should not change.
Do not write code until I approve.
```

Expected output:

- Decision log
- Unresolved assumptions
- Minimal implementation scope
- Test plan
- Verification commands
- Explicit approval request

---

## Slide 40 — Approve, Implement, Verify

After reviewing the contract:

```text
I approve the implementation contract.

Implement the smallest cohesive change.
Follow repository and path-specific instructions.
Run the approved tests and build.
Review the final diff and report remaining risks.
```

Verification ladder:

```text
Focused tests → integration tests → build → runtime check → diff review
```

---

## Slide 41 — Workshop Takeaway

> A prompt asks for an answer. Instructions shape recurring behavior. A skill packages a reusable workflow.

The `grill-me` skill changes the agent from:

```text
Request → Generate code
```

into:

```text
Request → Investigate → Interview → Decide → Plan → Approve → Implement → Verify
```
