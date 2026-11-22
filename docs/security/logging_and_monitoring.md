# Logging and monitoring (lab 7-8)

This document describes what RecipeSharingSystem logs, why, and how an operator is expected to use
those logs. It was written after analysing a (fictional, course-provided) incident at a library
system called Libraxis, where a weak, inconsistent application log and a much stronger web-server
log had to be cross-referenced by hand to reconstruct a credential-stuffing attack that escalated
into SQL injection, session-token theft, mass PII exfiltration and finally a dropped production
database. The design choices below exist specifically to avoid the gaps that investigation exposed.

## What the Libraxis incident showed was missing

Reconstructing that incident required correlating two logs by matching timestamps by hand, because:

- The application log used **six different timestamp formats across its own components**, none with
  a time zone, and had **no request/session identifier**, so two lines produced by the same HTTP
  request could not be tied together without guessing from context.
- It recorded only **successful** logins (`login ok`), never failed ones — so the credential-stuffing
  attempt that preceded the eventual successful login was invisible in that log, and only showed up
  in the separate web-server log.
- It had no source IP / client identifier on any line, so even the two SQL errors it did record
  could not be attributed to an attacker rather than to a bug or a legitimate admin query.
- Nothing about a response's *size* or *content* was recorded at the application layer, so there was
  no way to tell, from the application log alone, that two routine-looking "report generation" events
  were actually triggered in the middle of an active intrusion.

## Design

### Structured, single-format logging (Serilog)

Logging is implemented with [Serilog](https://serilog.net/) (`Serilog.AspNetCore`), configured entirely
from `appsettings*.json` under the `Serilog` section (see `RecipeSharingSystem.API/appsettings.json`):

- **One timestamp format, always with an offset** (`yyyy-MM-ddTHH:mm:ss.fffK`), used for every log line
  from every component — the first and most basic fix for the format chaos seen in the Libraxis log.
- **Two sinks**: console (for local development and container log collection) and a rolling daily file
  under `logs/app-.log` (`RecipeSharingSystem.API/logs/`, excluded from git — see `.gitignore`), retained
  for 90 days by default (`retainedFileCountLimit`). Adjust the retention period to match the
  organisation's actual log-retention / data-protection policy before deploying.
- **A correlation id on every line.** `RequestCorrelationMiddleware` (`RecipeSharingSystem.API/Extensions/`)
  assigns (or honours a client-supplied `X-Correlation-Id` header for) a per-request id, pushes it into
  Serilog's `LogContext`, and returns it in the response header. Every log line written anywhere while
  handling that request — the request-summary line from `UseSerilogRequestLogging()`, a security event,
  an unhandled exception — carries the same `CorrelationId` property, so a support engineer (or an
  incident responder) can filter the whole file down to one request or one user session instantly,
  instead of reconstructing it from timestamps.
- **A per-request summary line** (`app.UseSerilogRequestLogging()`) with method, path, status code and
  duration for every request — this is the equivalent of the web-server log in the Libraxis case, and it
  exists *in addition to* the security events below, specifically so that the two complementary views
  (what was requested vs. what the application did with it) are both available from a single system,
  instead of only being reconcilable after the fact from two different servers' logs.

### Named security events (`ISecurityEventLogger`)

Free-text log messages are easy to miss and hard to alert on. Security-relevant actions are additionally
recorded through a dedicated service, `ISecurityEventLogger`
(`RecipeSharingSystem.Core/Interfaces/Infrastructure/ISecurityEventLogger.cs`, implemented by
`SecurityEventLogger` in the Infrastructure project), which writes one entry per event with a fixed
`EventType` property and a fixed message template:

| EventType | Raised from | What it answers |
|---|---|---|
| `AuthenticationSucceeded` | `AuthService.Login` | Who logged in, and when. |
| `AuthenticationFailed` | `AuthService.Login` | Which account was targeted, and why the attempt failed (no account vs. wrong password) — the exact gap that let the Libraxis brute-force go unnoticed in the application log. |
| `AuthorizationDenied` | `ResourceAccessService` (ownership checks) and `PermissionAuthorizationHandler` (policy checks) | Which authenticated user was denied which resource/permission — repeated denials against different resources from one account are a classic sign of a compromised or probing account. |
| `RateLimitRejected` | the rate limiter's `OnRejected` callback in `Program.cs` | Which client/path combination is being throttled — an early warning the Libraxis log never gave (it only recorded *successful* logins). |
| `AdminOperation` | the `--reencrypt-pii` CLI command today; the natural extension point for any future admin endpoint (role changes, bulk exports, etc.) | Who performed a sensitive, high-blast-radius operation, and on what. |

**What is deliberately never passed to these methods, or logged anywhere else:** passwords (hashed or
plain), JWTs/refresh tokens, the AES encryption keys or key-ring material, and full request/response
bodies. The project does **not** add a generic "log every request body" middleware, precisely because
that is how systems end up with secrets sitting in a log file — the Libraxis `sessions` table leak in
the course scenario is a reminder of how dangerous it is to let tokens end up somewhere they can be
bulk-read. An email address used in a *failed* login attempt is logged, because it is the audit subject
of that specific event (which account is being targeted) and not a secret; if the organisation's privacy
policy requires it, this can be masked (e.g. logging only a hash or the domain) without losing the
event's usefulness.

### What to monitor / alert on

Using the events above, the following conditions are directly queryable and worth alerting on (exact
thresholds depend on real traffic volume and should be tuned, not copied blindly):

- Multiple `AuthenticationFailed` for the same `emailAttempted` within a short window (credential
  stuffing / brute force — the pattern that opened the Libraxis incident).
- Any `RateLimitRejected` burst from a single client key, especially against `/api/Auth/*`.
- `AuthorizationDenied` events against several *different* resources from the same user id in a short
  window (an account trying things it doesn't own — consistent with a stolen or hijacked session).
- An `AdminOperation` or any request to an admin-only route appearing immediately after an
  `AuthenticationSucceeded` for an account that has never produced admin operations before (the
  Libraxis incident's `/api/admin/dashboard` access right after the session-token exfiltration, with no
  separate admin login, is exactly this pattern).
- A spike in 5xx responses in the per-request summary line, especially from a single source — the
  earliest visible symptom of the Libraxis database-drop.

None of this requires a SIEM to start with: even `grep`/`jq` over `logs/app-*.log` for `"EventType":"AuthenticationFailed"`
grouped by `EmailAttempted` and time window answers the first bullet. A SIEM/log-shipping pipeline
(e.g. shipping the JSON-formatted file sink to an ELK/Grafana Loki stack) is the natural next step once
log volume makes manual review impractical, but is out of scope for this student project.

## Related documents

- [`identity_and_access.md`](./identity_and_access.md) — authentication/authorization design that the
  `AuthenticationFailed`/`AuthenticationSucceeded`/`AuthorizationDenied` events report on.
- [`backup_and_recovery.md`](./backup_and_recovery.md) — what happens *after* an incident like the
  Libraxis database drop: how the database is actually restored.
- [`security_policy.md`](./security_policy.md) — vulnerability register, including the logging gaps
  this document addresses.
