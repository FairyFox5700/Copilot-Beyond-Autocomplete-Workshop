# Slide Ideas — When Instruction Files Multiply

## Slide 42 — More Instructions Do Not Mean More Intelligence

Multiple `AGENTS.md` files—or equivalent scoped instruction files—can help in a monorepo.

They can also make an agent less predictable when they introduce:

- conflicting rules;
- unnecessary context;
- unclear scope;
- duplicated guidance;
- maintenance and debugging overhead.

> Add another instruction file only when a directory genuinely needs different guidance.

---

## Slide 43 — Problem 1: Instruction Conflicts

```text
Root AGENTS.md:       Use xUnit.
src/AGENTS.md:        Use NUnit.
tests/AGENTS.md:      Use MSTest.
```

Possible outcomes:

- The agent follows the wrong rule.
- It combines incompatible frameworks.
- Different files receive inconsistent tests.
- The result changes between Copilot surfaces.

**Workshop question:** Which instruction is authoritative, and why are test-framework rules in `src/` at all?

---

## Slide 44 — Problem 2: Context Overhead

```text
Root instructions
+ backend instructions
+ frontend instructions
+ test instructions
+ feature instructions
+ task prompt
+ source code
+ command output
```

Every relevant instruction consumes context before the agent solves the task.

Consequences:

- Less room for source code and requirements
- Important evidence competes with generic guidance
- Higher chance of missing a critical constraint
- More cost and latency

**Rule:** keep always-loaded guidance short; load specialized workflows only when relevant.

---

## Slide 45 — Problem 3: Unclear Scope

```text
repo/
├── AGENTS.md
├── src/
│   ├── AGENTS.md
│   └── API/
│       ├── AGENTS.md
│       └── Controllers/
└── tests/
    └── AGENTS.md
```

Ask:

- Which instructions apply to a controller test?
- What happens when one task changes API and test files?
- Which rule wins when two files disagree?
- Does the current IDE, CLI, or cloud agent discover nested files identically?

> Do not assume every coding-agent surface discovers, combines, or prioritizes nested instruction files in the same way.

---

## Slide 46 — Prefer Explicit Path Scope

For GitHub Copilot, make specialized scope visible when the surface supports path-specific instructions:

```markdown
---
applyTo: "demo/tests/**/*.cs"
---

- Use xUnit.
- Use exact decimal assertions for money.
```

Benefits:

- The intended scope is reviewable.
- The rule is attached to matching files.
- Repository-wide instructions remain small.
- Teams can search for all scoped rules in one directory.

**Caution:** support and discovery details can differ between Copilot surfaces; verify the client used in the workshop.

---

## Slide 47 — Problem 4: Duplication and Drift

A naming rule is copied into:

```text
AGENTS.md
src/AGENTS.md
tests/AGENTS.md
```

Six months later, only one copy is updated.

Now the agent receives different guidance depending on where it works.

**Better pattern:**

- Keep shared rules in one authoritative file.
- Keep scoped files limited to genuine differences.
- Link to maintained documentation instead of copying large sections.
- Review instruction changes like production code.

---

## Slide 48 — Problem 5: False Confidence

More instruction files do not guarantee better output.

The agent may still:

- follow outdated architecture guidance;
- treat an example as a mandatory rule;
- apply a local convention outside its scope;
- miss a file that was not loaded;
- follow instructions but misunderstand the task.

> Instructions guide model behaviour. Tests, builds, permissions, CI, and review provide evidence and enforcement.

---

## Slide 49 — Problem 6: Harder Troubleshooting

When Copilot produces incorrect code, investigate:

1. Which instruction files were available?
2. Which ones were actually referenced?
3. Did applicable rules contradict one another?
4. Was the expected scope supported by this Copilot surface?
5. Was the instruction too vague or outdated?
6. Did deterministic validation catch the mistake?

**Debugging technique:** ask Copilot to list the instructions relevant to the current file, then verify the answer against the repository and client UI.

---

## Slide 50 — Recommended Monorepo Starting Point

```text
repo/
├── AGENTS.md or .github/copilot-instructions.md
├── src/
│   ├── API/
│   └── Worker/
├── tests/
└── docs/
```

Start with one root file containing universal rules.

Add scoped guidance only when API, worker, tests, migrations, or another directory genuinely needs different behavior.

| Root instructions | Scoped instructions |
|---|---|
| Architecture principles | API-specific conventions |
| General C# conventions | Worker-specific conventions |
| Standard build and test commands | Local validation commands |
| Security principles | Framework-specific patterns |
| Shared coding standards | Directory-specific constraints |

---

## Slide 51 — Workshop Project: Deliberate Scope

```text
.github/
├── copilot-instructions.md
└── instructions/
    ├── checkout-domain.instructions.md
    ├── tests.instructions.md
    └── migrations.instructions.md
```

Why this structure works:

- Global file: .NET version, money, API stability, completion checks
- Domain file: checkout invariants only
- Test file: xUnit and deterministic test conventions only
- Migration file: EF Core history and deployment safety only

The same rule is not copied into every file.

---

## Slide 52 — Decision Checklist Before Adding a File

Ask:

1. Does this guidance apply to most repository tasks?
   - Put it in the root instructions.
2. Does it apply only to a clear path or file type?
   - Use a scoped instruction.
3. Is it a multi-step workflow used occasionally?
   - Use a skill or prompt file.
4. Is the information already discoverable from code or documentation?
   - Do not duplicate it.
5. Does the rule need deterministic enforcement?
   - Use tests, analyzers, CI, permissions, or hooks.
6. Can we explain which file is authoritative if two rules disagree?
   - If not, simplify before adding another file.

---

## Slide 53 — Participant Exercise: Audit the Hierarchy

Given:

```text
Root:  Use xUnit.
src:   Use NUnit.
tests: Use MSTest.
```

Ask participants to:

1. identify every conflict;
2. choose one authoritative shared rule;
3. remove duplicated guidance;
4. scope only genuine local differences;
5. define the test command that proves the result;
6. explain how they would verify which instructions Copilot used.

**Takeaway:** instruction architecture should reduce decisions, not create new ones.
