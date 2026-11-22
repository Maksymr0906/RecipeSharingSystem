# Identification, Authentication and Access Control — RecipeSharingSystem

| | |
| --- | --- |
| Version | 1.0.0 |
| Related | [`security_policy.md`](./security_policy.md), [`threat_model.md`](./threat_model.md), [`cryptography.md`](./cryptography.md), [`client_server_trust.md`](./client_server_trust.md) |

## 1. Identification

* A person is identified by a **unique e-mail address** (login) and a **unique user name** (public name). Both have unique database indexes and are also checked in `AuthService` / `UserService` before writing.
* The internal identifier is a GUID. It is taken from the `userId` claim of the JWT (`ICurrentUser`), never from the request body.

## 2. Authentication

| Method | Status | Notes |
| --- | --- | --- |
| Password (e-mail + password) | **Implemented** | bcrypt via `EnhancedHashPassword` (SHA-384 pre-hash, so the 72-byte limit does not truncate long passwords), work factor 12 (≈ 0.28 s per hash in the lab measurement), per-hash random salt. Password length 8–128, must differ from user name/e-mail. |
| JWT bearer token | **Implemented** | HMAC-SHA256, key ≥ 32 bytes from secrets, lifetime from `JwtOptions:ExpiresHours`, `ClockSkew` 30s. Carries only `userId` (roles are re-resolved server-side on each request, not trusted from the token). Issuer/audience are validated (lab 6) — see [`client_server_trust.md`](./client_server_trust.md). |
| Login error handling | **Implemented** | Unknown e-mail and wrong password give the same `401 Invalid email or password.`; for an unknown e-mail a dummy bcrypt verification is performed so response time does not reveal whether the account exists. |
| Throttling | **Implemented** | 5 requests per minute per IP and endpoint on `/api/Auth/*` → `429`. |
| Multi-factor authentication (TOTP, RFC 6238) | **Designed, not implemented** | Planned: `TotpSecret` (encrypted with `IFieldEncryptor`, context `User.TotpSecret`) and `TotpEnabled` on `User`; enrolment endpoint returns an `otpauth://` URI; login with enabled MFA returns a short-lived "MFA required" token, the second step checks a 6-digit code (30 s step, ±1 window, replay protection by remembering the last accepted step) and only then issues the full JWT; 10 one-time recovery codes stored as bcrypt hashes. Mandatory for the `Admin` role. |

## 3. Access control

Model: **RBAC for operation types + ownership for individual objects**.

1. *Authenticated?* — `[Authorize]` / permission policy on the endpoint.
2. *Allowed operation?* — role → permission mapping from configuration (`PermissionAuthorizationHandler`).
3. *Allowed object?* — service layer: the caller must own the object, or hold the generic administrator permission (`Update` / `Delete`, which only `Admin` has). Implemented once in `ResourceAccessService.EnsureOwnerOrPermissionAsync`.

| Resource | Guest | User (own objects) | User (others' objects) | Admin |
| --- | --- | --- | --- | --- |
| Recipes, ingredients, instructions, reviews — read | ✔ | ✔ | ✔ | ✔ |
| Recipe / review — create | ✘ | ✔ (author = caller, from token) | — | ✔ |
| Recipe / review / instruction — update, delete | ✘ | ✔ | ✘ 403 | ✔ |
| User profile — read, update, delete | ✘ | ✔ (self) | ✘ 403 | ✔ |
| Favorites — read, add, remove | ✘ | ✔ (self) | ✘ 403 | ✔ |
| Role change | ✘ | ✘ | ✘ | not exposed via API (no role field in DTOs) |

Failures map to `401` (not authenticated), `403` (authenticated but not allowed), `404` (object absent), `400` (validation). Error bodies never contain stack traces or internal messages.

## 4. Residual risks

* Throttling is per IP only; there is no per-account lockout. Behind a proxy, forwarded headers must be configured or all clients share one counter.
* JWTs cannot be revoked before expiry (no server-side session store or deny list); keep the lifetime short.
* Registration reveals whether an e-mail is already taken (needed for usability; mitigated only by throttling).
* Instructions not yet attached to any recipe have no owner and can be edited by any user with the permission (they exist only briefly between creation and recipe creation).
* The seed users contain bcrypt hashes with cost 11, so their verification is faster than the dummy hash used for timing equalisation.
* MFA is designed but not implemented.
* V3 (image upload path traversal) is still open.
