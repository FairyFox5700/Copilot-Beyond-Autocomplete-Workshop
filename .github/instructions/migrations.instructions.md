---
name: "EF Core migration safety"
description: "Safety rules for generated and reviewed database migrations."
applyTo: "demo/src/Store.Checkout.Persistence/Migrations/**/*.cs"
---

- Treat existing migrations as immutable history; create a new migration for schema changes.
- Review both `Up()` and `Down()` before recommending approval.
- Call out destructive operations such as dropped columns, tables, or indexes.
- Check compatibility with application versions that may still be deployed.
- Do not generate or apply a migration unless the task explicitly requests it.
- Never infer permission to update a database from permission to edit migration code.
