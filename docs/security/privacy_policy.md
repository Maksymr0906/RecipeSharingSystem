# Privacy Policy — RecipeSharingSystem

| | |
| --- | --- |
| Title | Privacy Policy — RecipeSharingSystem |
| Version | 1.0.0 |
| Status | Draft |
| Classification | Internal |
| Last updated | 2026-09-25 |
| Owner | Maksym R. (project developer/owner) |
| Related documents | [`security_policy.md`](./security_policy.md), [`threat_model.md`](./threat_model.md) |

> This document is kept separate from `security_policy.md` because RecipeSharingSystem involves multi-user interaction (registration, profiles, reviews, favorites, user administration) — under this model, a standalone document can later be shown publicly to users for consent, while `security_policy.md` remains an internal document.

## 1. Categories of Data

The system processes the following data associated with a user account (the `User` entity, `Backend/RecipeSharingSystem.Data/Entities/User.cs`):

| Field | Required | Purpose |
| --- | --- | --- |
| `UserName` | required | identification in the UI, authentication |
| `Email` | required | authentication (login), unique account identification |
| `PasswordHash` | required | authentication; stored only as a BCrypt hash, never in plaintext |
| `FirstName`, `LastName` | optional | profile display |
| `DateOfBirth` | optional | not currently used by any implemented feature |
| `PostalCode` | optional | not currently used by any implemented feature |

The system additionally stores content linked to a user, which is also personal data in a broader sense:

* review text (`Review.Content`) and ratings (`Review.Rating`), linked to `UserId`;
* the list of favorited recipes (`UserFavoriteRecipe`);
* recipe authorship (`Recipe.AuthorId`).

## 2. Data Sources

All data comes directly from the user:

* during registration (`POST /api/Auth/register`) — `UserName`, `Email`, password (converted to a hash and never stored in its original form);
* while editing a profile (`PUT /api/Users/{id}`) — `FirstName`, `LastName`, `DateOfBirth`, `PostalCode`, updated `UserName`/`Email`;
* while interacting with recipes — review text, favorited recipes.

The system does not receive user data from any external source and is not integrated with third-party authentication providers (OAuth/SSO) or any other external service.

## 3. Purpose of Processing

* `Email` + `PasswordHash` — authentication only.
* `UserName` — public display of recipe/review authorship.
* `FirstName`, `LastName` — display in the user's profile.
* `DateOfBirth`, `PostalCode` — reserved fields in the data model, but not currently used by any implemented business function; per the data-minimization principle (Section 10), they should either be tied to a concrete future feature or removed from the schema.

## 4. Storage

* All personal data is stored in the `Users` table of the `recipe_sharing_system` MySQL database.
* `PasswordHash` is stored only as the output of `BCrypt.Net.BCrypt.EnhancedHashPassword` — the raw password is never stored by the system in any form.
* Database backups are not currently implemented (out of scope for the current lab assignment).

## 5. Access to Data

* Only backend components (`RecipeSharingSystem.API`/`.Application`/`.Persistence`), running on the trusted server side, have access to personal data.
* **Gap identified while preparing this document (corresponds to V2 in `security_policy.md` §9 and in `threat_model.md`):** the `GET /api/Users/{id}` endpoint currently has no `[Authorize]` attribute, meaning any API call — including an unauthenticated one — can retrieve the `Email`, `FirstName`, `LastName`, `DateOfBirth`, and `PostalCode` of any user by GUID. This violates the access-restriction principle stated in this document and must be fixed by adding authorization to this endpoint.
* Once fixed, access to a full user profile should be granted only to the user themself (matching the id against the token) or to an account with the `Admin` role.

## 6. Sharing with Third Parties

Personal data is not shared with any external service or third party — the project is not integrated with any external API (no email delivery, payment systems, analytics, etc.).

## 7. Retention Period and Deletion

* Automatic deletion or anonymization of personal data is not currently implemented: `UsersController` has no account-deletion endpoint.
* This is recorded as an open item: before any public use of the system, it is recommended to add the ability for a user to delete or anonymize their account (e.g., clearing `Email`/`FirstName`/`LastName`/`DateOfBirth`/`PostalCode` while retaining an anonymized author placeholder for already-published recipes/reviews, so as not to break the `AuthorId`/`UserId` relationships).

## 8. Logging of Sensitive Data

The plaintext password, the password hash, and issued JWT tokens must never be written to application logs (consistent with `security_policy.md` §7).

## 9. Cryptographic Protection

* Passwords — BCrypt (`EnhancedHashPassword`/`EnhancedVerify`), a modern and acceptable algorithm for password storage.
* Transport — the client↔API connection is protected via `UseHttpsRedirection()`; for production deployment, a properly configured TLS certificate is mandatory (out of scope for the current lab assignment, since the system is not deployed publicly).

## 10. Data Minimization

The system should collect only the personal data actually required by implemented features. As of this document, the `DateOfBirth` and `PostalCode` fields are collected but not used by any business function (there is no age restriction, delivery, or geolocation feature) — it is recommended to either define a concrete purpose for them as the project develops further, or remove them from the `User` schema in line with the data-minimization principle.

## 11. User Rights

Within the current implementation, a user can:

* view and edit their own profile (`PUT /api/Users/{id}`, restricted to themself or an Admin once V2 is fixed);
* view their own list of favorited recipes and reviews via the corresponding endpoints.

The ability to self-delete an account and its personal data is an open item (see Section 7).

## 12. Related Documents

* [`security_policy.md`](./security_policy.md) — technical security requirements, including the vulnerability register (V1–V5), some of which also affect data privacy (notably V1 and V2).
* [`threat_model.md`](./threat_model.md) — the STRIDE threat model, including the Information Disclosure category for elements that store personal data (`D1 MySQL Database`, `P2 Web API`).