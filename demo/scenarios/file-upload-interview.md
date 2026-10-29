# Presenter Scenario — File Upload Interview

## Starting request

```text
Use the grill-me skill.

Build an API endpoint that lets users upload files.
```

## Suggested presenter answers

Use these only as the live interview reaches each topic.

| Decision | Presenter answer | Why it changes implementation |
|---|---|---|
| Allowed content | PDF and PNG, maximum 10 MB | Validation, request limits, tests |
| Content verification | Inspect file signatures and parse enough content to reject malformed files; do not trust extension or MIME type | Security boundary and validation design |
| Authorization | Authenticated users may upload only to their own account | Identity, ownership, tenant tests |
| Duplicate handling | SHA-256 per owner; return the existing file identifier for an identical upload | Hashing, idempotency, API response |
| Storage | Stream content to private object storage; store metadata in the application database | Service boundary and failure handling |
| Malware | Upload into quarantine, return `202 Accepted`, and expose the file only after asynchronous scanning succeeds | State model, background processing, status endpoint |
| Storage failure | Do not create a completed metadata record; return a safe `503` response and log a correlation ID | Recovery and observable failure behavior |
| Metadata failure after upload | Delete the uncommitted object when possible; otherwise record cleanup work | Compensation and operational recovery |
| Tests | Allowed PDF/PNG, invalid signature, malformed file, oversize file, unauthorized owner, duplicate, storage failure, scan failure, and clean release | Definition of done |

## Expected unresolved decisions

A good interview should still call out choices the presenter has not supplied, such as:

- exact object-storage provider;
- retention and deletion policy;
- malware-scanner product and timeout;
- rate limits;
- whether clients poll or receive an event when scanning finishes.

Do not let Copilot silently choose these.

## Approval prompt

```text
Now summarize the decisions we made.
Identify assumptions I failed to resolve.
Propose the smallest implementation and a test plan.
Name the files that should and should not change.
Do not write code until I approve.
```
