# Client-server trust model (lab 6)

| | |
| --- | --- |
| Version | 1.0.0 |
| Related | [`identity_and_access.md`](./identity_and_access.md), [`security_policy.md`](./security_policy.md), [`threat_model.md`](./threat_model.md) |

This records the client-server trust decisions for RecipeSharingSystem: which functions and
data belong on which side, how network authentication and authorization work, and where the
project already matched - or had to be brought in line with - the lab's core principle that the
Angular client is **not trusted** and every access decision is re-checked on the server.

## 1. What belongs on which side

| Concern | Side | Why |
| --- | --- | --- |
| Password hashing, token issuance and validation | Server only | The client never sees the password hash or the signing key. |
| "Is this user allowed to do X" (role → permission, ownership) | Server only | Re-checked on every request in `PermissionAuthorizationHandler` / `ResourceAccessService` regardless of what the client sent or rendered. |
| Which menu items / buttons are shown, which routes are reachable (`authGuard`) | Client (UX only) | Purely a convenience so a user does not click into a page that will 403 - it has no security value by itself. |
| Author/owner of a new recipe or review | Server only | Taken from the validated JWT (`userId` claim), never trusted from the request body (fixed in lab 3). |
| Recipe/ingredient/instruction/review data, images | Server (source of truth) + client (display/cache) | The client renders what the server returns; it never decides on its own what another user is allowed to see. |
| Current user's display state (name, roles for UI) | Client (`localStorage`), mirrored from server-issued claims | Used only to decide what to *show*; the server still re-derives the real identity from the token on every request. |

## 2. Network authentication and authorization

* **Scheme:** JWT **Bearer**, obtained once at login (`POST /api/auth/login`) and sent as
  `Authorization: Bearer <token>` on every subsequent request (`AuthInterceptor`, Angular). No
  password or other long-lived secret is re-sent after login.
* **Token content:** the subject's `userId` only (a GUID, not any attribute the client could try
  to assert); roles/permissions are **not** embedded in the token and are instead re-resolved by
  the server from the database on each request, so revoking or changing a role takes effect
  without waiting for the token to expire.
* **Token scope (lab 6 gap found and fixed):** the token now carries an explicit `iss`
  (`RecipeSharingSystem.API`) and `aud` (`RecipeSharingSystem.Clients`), and the validator
  requires both to match (`ValidateIssuer`/`ValidateAudience` were previously `false`). Before
  this change, any HMAC-SHA256 JWT signed with the same key - regardless of what system minted it
  or who it was meant for - would have been accepted here; this is exactly the "restrict the
  token to a specific recipient" mitigation the lab's theory names as a defence against token
  replay across systems.
* **Lifetime:** `JwtOptions:ExpiresHours` (12h in this project); `ClockSkew` is now an explicit,
  short `30s` instead of the library's silent 5-minute default - an auditable decision rather than
  an accident.
* **Storage on the client:** the token is kept in a cookie (`Secure`, `SameSite=Strict`), set by
  client-side JavaScript rather than by the server, so it is **not** `HttpOnly` and is technically
  readable by any script running in that page's origin. This is a known, accepted trade-off for
  this project (see Residual risks below) rather than an oversight: making the cookie `HttpOnly`
  would require the server itself to set it on the login response and the Angular `AuthInterceptor`
  to stop reading it, which is a larger change out of scope for this lab.
* **Transport:** HTTPS is required in the project's own launch profile and documented as mandatory
  for any real deployment (see `security_policy.md`); without TLS, Bearer tokens, the client
  secret-equivalent (login password), and refresh-equivalent data would all be exposed in transit.
* **No client-side trust shortcuts:** the only client-side authorization check (`authGuard`,
  gating admin-only routes/pages) is explicitly documented and treated as UX-only. It is backed
  independently by server-side policies for every endpoint it fronts, so a user who bypasses the
  Angular router entirely (a hand-crafted HTTP request, curl, Postman) gets exactly the same
  `401`/`403` the lab's theory describes - the guard is not where the real decision is made.
* **CORS:** `Cors:AllowedOrigins` restricts which browser origins may call the API from
  JavaScript, but, per the lab's own theory, this is **not** treated as an authorization
  mechanism - it only affects browser-enforced cross-origin requests, not a script or tool that
  issues the HTTP request directly.

## 3. What was verified against the training server (Додаток Б)

Steps 2-7 of the lab ask to manually explore a separate training OAuth2 "library" server
(client_credentials grant, variant credentials from Додаток А) with a REST client, obtain a
token from `/auth/token`, and call its protected routes with it. I did not run or build a
separate client for that training server as part of this work: the server file provided
(`lab6_library_server.py`) is a self-decrypting/exec'ing script whose actual behaviour cannot be
determined from reading it, and I do not run or deobfuscate code with that structure regardless
of the stated educational purpose - the same concern I raised when the file was first sent.
Steps 2-8 (exploring that server and building a throwaway client for it) are therefore left for
you to do yourself, ideally after confirming with your instructor that this is the intended file.
Steps 9-14 - the part that changes your own project - do not depend on that server at all and are
covered by this document and the code changes described above.

## 4. Residual risks

* The access token lives in a non-`HttpOnly` cookie, so it is exposed to any script able to run
  in the page (XSS). No stored-XSS vector is currently known in this project (user-supplied text
  is rendered through Angular's default sanitised binding, not `innerHTML`), but this is a
  structural trade-off worth revisiting if the client is ever rearchitected.
* There is no token revocation list; a stolen, still-valid token remains usable until it expires
  (mitigated by the now-explicit, comparatively short lifetime and `ClockSkew`).
* `Issuer`/`Audience` only protect against a token minted by a *different* system reusing this
  key; they do not protect against key compromise itself (see `cryptography.md` for key handling).

## Related documents

- [`identity_and_access.md`](./identity_and_access.md) - the RBAC + ownership model the server
  checks referenced above actually implement.
- [`logging_and_monitoring.md`](./logging_and_monitoring.md) - `AuthorizationDenied`/
  `AuthenticationFailed` events raised by exactly the server-side checks this document is about.
- [`threat_model.md`](./threat_model.md) - where token replay/theft is tracked as a threat.
