# Presenter Scenario — Instruction Hierarchy Audit

## Deliberately bad monorepo

```text
repo/
├── AGENTS.md                    # Use xUnit; target .NET 8
├── src/
│   ├── AGENTS.md                # Use NUnit; target .NET 9
│   └── API/
│       ├── AGENTS.md            # All endpoints return 200
│       └── Controllers/
└── tests/
    └── AGENTS.md                # Use MSTest; target .NET 8
```

## Ask Copilot

```text
Audit this instruction hierarchy.

Identify:
- contradictory rules;
- duplicated guidance;
- unclear scope;
- instructions that belong in deterministic tooling;
- the smallest maintainable replacement structure.

Do not modify files.
```

## Expected findings

- Three incompatible test-framework rules
- Conflicting target-framework versions
- An unsafe HTTP-status rule
- No clear precedence or authority
- Framework choices placed in directories where they do not belong

## Recommended redesign

```text
repo/
├── .github/
│   ├── copilot-instructions.md
│   └── instructions/
│       ├── api.instructions.md
│       └── tests.instructions.md
├── src/
└── tests/
```

### Repository-wide

- Target .NET 8
- Use existing architecture and package versions
- Run focused tests, full tests, and build before completion

### Test-specific

```yaml
applyTo: "tests/**/*.cs"
```

- Use xUnit
- Follow established naming and assertion conventions

### API-specific

```yaml
applyTo: "src/API/**/*.cs"
```

- Use correct HTTP semantics
- Preserve the established error contract
- Keep business logic outside controllers

## Discussion prompt

> If a rule must never be violated, why is it only written as model guidance instead of being enforced by an analyzer, test, CI rule, permission, or deployment policy?
