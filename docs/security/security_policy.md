# Security Policy — RecipeSharingSystem

| | |
| --- | --- |
| Title | Security Policy — RecipeSharingSystem |
| Version | 1.0.0 |
| Status | Draft |
| Classification | Internal |
| Last updated | 2026-09-25 |
| Owner | Maksym R. (project developer/owner) |
| Related documents | [`project-policy.md`](../policies/project-policy.md), [`privacy_policy.md`](./privacy_policy.md), [`threat_model.md`](./threat_model.md), [`business-logic.md`](../architecture/business-logic.md), [`identity_and_access.md`](./identity_and_access.md), [`client_server_trust.md`](./client_server_trust.md), [`integrity_and_distribution.md`](./integrity_and_distribution.md), [`logging_and_monitoring.md`](./logging_and_monitoring.md), [`backup_and_recovery.md`](./backup_and_recovery.md) |

> This policy extends the top-level "Security Policy" section (§9) of `project-policy.md` and details concrete requirements for the project's actual stack: **ASP.NET Core 8 (Clean Architecture: API / Application / Data / Infrastructure / Persistence)**, **Angular 16 SPA**, **MySQL (EF Core + Pomelo)**, **JWT Bearer** authentication, **BCrypt** password hashing.
>
> This document was written after reviewing the actual repository code as of 2026-09-25. Several requirements therefore reference real files directly and call out issues already found during that review — these are recorded honestly in Section 9 "Vulnerability Management" rather than hidden.

## Table of Contents

1. Purpose
2. Scope
3. Roles and Responsibilities
4. Security Principles
5. Secure Development Environment
6. Software Protection
7. Secure Implementation
8. Security Verification
9. Vulnerability Management
10. Exceptions
11. Related Documents
12. Final Provisions

---

## 1. Purpose

The purpose of this policy is to establish mandatory rules for the secure development of **RecipeSharingSystem**, aimed at:

* protecting users' personal data (email, first/last name, date of birth, postal code) stored in the `Users` table;
* protecting credentials (password hashes) and authentication JWTs;
* protecting configuration secrets (the MySQL connection string, the JWT signing key);
* protecting the source code and user-uploaded images from unauthorized modification;
* establishing a process for detecting, classifying, and remediating vulnerabilities.

The policy applies to the full development lifecycle — from local development through any future deployment.

## 2. Scope

The policy applies to:

* `Backend/` — all five solution projects: `RecipeSharingSystem.API`, `.Application`, `.Data` (Core), `.Infrastructure`, `.Persistence`;
* `Frontend/recipe-sharing-system` — the Angular SPA;
* the `recipe_sharing_system` MySQL database;
* the image file storage (`Backend/RecipeSharingSystem.API/Images`, served via the static-files middleware at `/Images`);
* configuration files (`appsettings.json`, `appsettings.Development.json`);
* the project's GitHub repository (`master` branch).

**Out of scope:** production hosting/server infrastructure and third-party services — the project is not currently deployed publicly and is not integrated with any external email, payment, or other API.

Mandatory for the project developer and for any AI agents that modify code or documentation.

## 3. Roles and Responsibilities

The project is currently developed by a single person, so roles are simplified:

* **Project Owner / Developer** — responsible for complying with this policy, approves exceptions, and responds to vulnerabilities.
* **Reviewer** — not formally applied yet (no other contributors); once collaborators join, any change touching authentication, authorization, or configuration must be reviewed before merging into `master`.
* **AI agent** — may propose or make changes to code/documentation but does not independently decide to weaken security requirements (e.g., removing `[Authorize]`, adding secrets to source).

Separately from the policy roles above, the *application itself* defines runtime roles — `Guest` / `User` / `Admin` — enforced server-side on every request (detailed in `business-logic.md`, §2 and §6–7). This policy requires that a user's role always be determined by the server from the token/database, never trusted from a value supplied by the client.

## 4. Security Principles

* **Least privilege.** Authorization is built on permission-based policies (`CreatePolicy`, `ReadPolicy`, `UpdateRecipePolicy`, etc., in `ApiExtensions.cs`), where each role (`Admin`, `User`, `Guest`) maps to an explicit list of permissions in configuration (`AuthorizationOptions.RolePermissions`).
* **Secure by default / deny by default.** Any endpoint that mutates data must carry an explicit `[Authorize(Policy = "...")]` attribute; its absence is treated as a defect, not an "open by default" state (see the identified exception in Section 9).
* **No secrets in source code.** Secrets (the DB password, the JWT signing key) must not be stored in plaintext in configuration files that are committed to Git.
* **Passwords are never stored or logged in plain text.** Passwords are hashed with BCrypt (`PasswordHasher.cs`, `BCrypt.Net.BCrypt.EnhancedHashPassword`); the hash is never returned to the client and never appears in logs.
* **Server-side authorization is authoritative.** Hiding UI elements in Angular (e.g., an admin panel) is a UX improvement only, never a security mechanism; the final permission check is always performed by the API (consistent with `business-logic.md` §10).
* **Fail securely.** Validation errors (`FluentValidation.ValidationException`) are handled explicitly and return a controlled message; unexpected exceptions must not leak implementation details or stack traces to the client outside the Development environment.

## 5. Secure Development Environment

* **Repository:** GitHub, `master` branch; currently a single owner — branch protection (no direct pushes, mandatory PR review) is recommended once collaborators are added.
* **Git workflow:** per `project-policy.md` §12 — commits use the `[TYPE] description` convention; a commit must never mix unrelated changes with secrets.
* **Configuration and secrets:** `JwtOptions` and the connection string are read through ASP.NET Core's standard configuration mechanism (`IOptions<T>` / `IConfiguration`), which natively supports overriding via environment variables or `dotnet user-secrets` — **production values must come exclusively from there**, not from `appsettings.json`, which is tracked by the repository.
* **Dependencies:** NuGet packages (`Microsoft.EntityFrameworkCore`, `Pomelo.EntityFrameworkCore.MySql`, `BCrypt.Net-Next`, `Microsoft.AspNetCore.Authentication.JwtBearer`, `FluentValidation`, `AutoMapper`, `SixLabors.ImageSharp`, `Swashbuckle.AspNetCore`) and npm packages (Angular 16, `jwt-decode`, `ngx-cookie-service`, `marked`/`ngx-markdown`) are added only from official registries (nuget.org, npmjs.org). Before adding a new dependency, check for known vulnerabilities (`dotnet list package --vulnerable`, `npm audit`).
* **CI/CD:** the repository currently has no automated pipeline configured (builds/tests are run locally via `dotnet build`, `ng build`); adding a GitHub Actions workflow to build both backend and frontend on every push/PR is recommended — recorded as an open item in Section 8.
* **AI agents:** must follow this policy and `project-policy.md`; may not independently weaken authentication/authorization checks or introduce hardcoded secrets.

## 6. Software Protection

* **Source code protection:** repository access is currently limited to the owner; once the team grows, write access should be limited to those who need it, and critical branches protected.
* **Configuration secrets:** the file `Backend/RecipeSharingSystem.API/appsettings.json` **is currently tracked by Git and not excluded via `.gitignore`**, and it contains the MySQL password (`password=1111`) and the JWT signing key in plaintext. This violates the principle in Section 4 and is recorded as a critical item in Section 9 — the file should hold only local-development placeholders/defaults, while real values must be supplied via environment variables and never committed.
* **Seed data:** `SeedDataOptions` in `appsettings.json` contains demo accounts (including `admin@gmail.com`) with pre-computed BCrypt hashes. These credentials are intended only for local demo/lab purposes; they must not be treated as real production accounts. Publishing the hashes in the repository is not equivalent to a password compromise (BCrypt is resistant to offline brute-forcing within a reasonable time), but they should still be treated as test data, not trusted credentials.
* **Images:** the `Images/` directory is intended solely for validated images served statically; it must never become a store for arbitrary files (see the upload-validation requirement in Section 7).
* **Backups (lab 7-8):** daily `mysqldump` logical backups with a SHA-256 integrity checksum verified before every restore, optional at-rest encryption of the dump, and a documented RPO/RTO — see [`backup_and_recovery.md`](./backup_and_recovery.md) and `scripts/backup/`. A full backup/restore drill (including a deliberately tampered backup being correctly rejected) was executed against a representative database and is documented there; the operator must still run the same drill against the real production database and schedule the scripts before relying on them.
* **Release integrity and authenticity (lab 5):** published release artifacts (the compiled API output, the production Angular bundle) are hashed (SHA-256) and the resulting manifest is signed with an RSA-3072-PSS private key held only by the project owner; `RecipeSharingSystem.ReleaseIntegrity verify` checks a release against the committed public key before it is deployed. Full rationale and the chosen distribution model (source-available, self-hosted, SaaS-style server-side enforcement — no client-side license check) are in [`integrity_and_distribution.md`](./integrity_and_distribution.md).

## 7. Secure Implementation

* **Input validation.** FluentValidation is used (`RegisterRequestDtoValidator`, `LoginRequestDtoValidator`, etc.). `UpdateUserRequestDtoValidator` now has optional-field rules (user name ≤ 50, e-mail ≤ 100, first/last name ≤ 50, postal code ≤ 20, date of birth 1900-01-01 … today). The validators for recipes, reviews and similar DTOs are still empty and must be filled in.
* **Authentication.** JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`); tokens are signed with HMAC-SHA256 using a symmetric key from configuration; lifetime and signature are validated. `ValidateIssuer`/`ValidateAudience` are enabled (lab 6) against `JwtOptions:Issuer`/`JwtOptions:Audience`, so a token minted by a different system (even one that somehow shared or recovered this signing key) is rejected rather than accepted just because the signature checks out.
* **Client-server trust (lab 6).** The Angular client is treated as untrusted: every authorization decision (role/permission, ownership) is re-checked server-side on each request regardless of what the client rendered or sent; the one client-side role check (`authGuard`, admin routes) is UX-only and is backed by the same server-side policies as everything else. See [`client_server_trust.md`](./client_server_trust.md) for the full model, including the issuer/audience fix above and what is deliberately left server-side vs. client-side.
* **Authorization.** A custom permission-based system: the `Role → Permission` mapping is loaded from configuration and enforced by `PermissionAuthorizationHandler` via `[Authorize(Policy = "...")]`. **Mandatory requirement:** every endpoint that reads or modifies data belonging to a specific user, or performs a protected operation, must carry an explicit `[Authorize]` attribute; its absence is treated as a security defect (a concrete instance of this is recorded in Section 9).
* **Passwords.** Hashed with BCrypt (`EnhancedHashPassword`/`EnhancedVerify`) — an acceptable, modern algorithm. Work factor 12. Passwords must be 8–128 characters and must not equal the user name or e-mail (`RegisterRequestDtoValidator`).
* **File uploads (`ImagesController`).** The file name (`fileName`) comes from the client and is used directly to build the server-side path via `Path.Combine` without sanitization, and the extension is taken from the uploaded file's original name without checking it against an allow-list. **Requirement:** `fileName` must reject path separators and `..` sequences (or be ignored entirely and generated server-side, e.g. as a GUID), and the extension/content must be validated against an explicit allow-list (`.jpg`, `.jpeg`, `.png`, `.webp`) with confirmation that the content is actually an image (e.g. by attempting to decode it with the already-referenced `SixLabors.ImageSharp`).
* **CORS.** Allowed origins come from `Cors:AllowedOrigins` (default `http://localhost:4200`); set the real frontend origin before any public deployment.
* **Error handling.** Controllers explicitly catch `ValidationException` and return `BadRequest`. For unhandled exceptions, it must be confirmed that detailed error pages/stack traces are disabled outside the Development environment (`app.Environment.IsDevelopment()` is already used for Swagger — the same approach should be applied to error handling).
* **Logging and monitoring (lab 7-8).** Structured logging via Serilog with a single timestamp format, a per-request correlation id, and named security events (`ISecurityEventLogger`: authentication success/failure, authorization denial, rate-limit rejection, admin operations) — see [`logging_and_monitoring.md`](./logging_and_monitoring.md). Passwords, password hashes, JWT/refresh tokens, and encryption keys must never be written to application logs; no generic request/response-body logging middleware is used precisely to prevent that.

## 8. Security Verification

* **Automated tests.** The repository currently has no dedicated test project — per `project-policy.md` §11, adding unit/integration tests covering authentication, authorization (including the ownership rule and admin override from `business-logic.md` §7), and input validation is recommended.
* **Code review.** With a single developer, self-review before each commit is required against a short checklist: no new secrets in code/configuration; every endpoint that reads or modifies a specific user's data has `[Authorize]`; no ownership check has been removed or bypassed.
* **Dependency scanning.** Run `dotnet list package --vulnerable` and `npm audit` before every release/lab submission.
* **Static analysis.** Built-in Roslyn analyzers and nullable-reference warnings are used (`<Nullable>enable</Nullable>` in every `.csproj`) — new compiler warnings must not be ignored.
* **CI:** open item — add a GitHub Actions workflow to automatically build the backend (`dotnet build`) and frontend (`ng build`) on every push/PR.
* **Release gate.** A change must not land on `master` if it: adds a secret in plaintext; removes an existing `[Authorize]` attribute; or removes an ownership check on a resource.

## 9. Vulnerability Management

**Process:** register → classify by severity (Critical / High / Medium / Low) → assign an owner (the repository owner) → remediate → re-verify → update this document, `threat_model.md`, or `business-logic.md` if the issue reveals a gap in them.

**Register compiled while preparing this policy** (verified against the actual repository code as of 2026-09-25; the *Status* column was updated during labs 3 and 4):

| # | Finding | Severity | Component | Status |
| - | ------- | :------: | --------- | ------ |
| V1 | The DB password and JWT signing key are committed in plaintext | **Critical** | `appsettings.json` | **Mitigated.** Secrets removed from the file; they come from user-secrets / environment variables and the API refuses to start without them (JWT key ≥ 32 bytes, valid encryption key ring). **The old values remain in Git history and must be rotated.** |
| V2 | `UsersController.GetUserById` and `UpdateUser` have no `[Authorize]` | **Critical** | `UsersController.cs` | **Fixed.** `[Authorize]` on the controller plus a "self or administrator" check in `UserService` (see `identity_and_access.md`). |
| V3 | `ImagesController.CreateImage` builds the file path from a client-supplied `fileName` | High | `ImagesController.cs` | **Open.** Not part of labs 3–4; remains the next item to fix. |
| V4 | The CORS policy allows any origin | Medium | `Program.cs` | **Fixed.** `WithOrigins(Cors:AllowedOrigins)`, default `http://localhost:4200`. |
| V5 | Minimum password length is 5 characters | Low | `RegisterRequestDtoValidator.cs` | **Fixed.** 8–128 characters, password must differ from user name and e-mail. |
| V6 | No ownership check when changing recipes, reviews, instructions, favorites (any authenticated user with `Update`/`Delete` permission could change anyone's data); `CreateReview` trusted `UserId` from the body | High | `RecipeService`, `ReviewService`, `InstructionService`, `UserFavoriteRecipesService` | **Fixed.** Hybrid RBAC + ownership via `ResourceAccessService`; the author is always taken from the token. |
| V7 | `UserFavoriteRecipesController` had no `[Authorize]` | High | `UserFavoriteRecipesController.cs` | **Fixed.** |
| V8 | Registration did not assign a role; login errors were unhandled (`KeyNotFoundException` → 500, user enumeration through different messages) | Medium | `AuthService` | **Fixed.** `User` role assigned; one generic message for unknown e-mail and wrong password, plus a dummy hash to equalise timing. |
| V9 | No unique constraint on `Email` / `UserName` | Medium | `UserConfiguration` | **Fixed** (unique indexes + application-level check). Requires recreating the database. |
| V10 | Personal data (`FirstName`, `LastName`, `DateOfBirth`, `PostalCode`) stored in clear text | Medium | `users` table | **Fixed** by AES-256-GCM field encryption (see `cryptography.md`). `Email` stays in clear text because it is a login key. |
| V11 | No throttling of login/registration | Medium | `AuthController` | **Mitigated.** 5 requests/min per IP and endpoint. Per-account lockout and MFA are not implemented. |
| V12 | `InstructionService.DeleteInstructionAsync` deleted through the ingredient repository (pre-existing bug) | Low | `InstructionService.cs` | **Fixed.** |

Known remaining weaknesses are listed in `identity_and_access.md` (§ Residual risks) and `cryptography.md` (§ Limitations). Other FluentValidation validators (recipes, reviews, …) are still empty — pre-existing and not yet addressed.

These items are also the input to `threat_model.md`, where each one is tied to specific STRIDE threats (the `V1`–`V5` labels in the threat model correspond to this table; V6–V12 were found during labs 3–4).

## 10. Exceptions

Since the project is currently a student lab assignment developed by a single person, with no public production deployment, a temporary deviation from parts of Sections 6–7 is allowed:

* **V1 (secrets in configuration)** and **V4 (open CORS)** are acceptable exclusively in the local development environment; the project owner knowingly accepts this risk until any deployment beyond `localhost`, at which point the exception no longer applies without further approval.
* **V2 (missing authorization)** and **V3 (path traversal on upload)** are not a temporarily acceptable risk but implementation defects; the exception does not cover them, and they must be fixed regardless of deployment environment.

Any new exception must be justified in writing, have a defined expiration, and be approved by the project owner.

## 11. Related Documents

* [`project-policy.md`](../policies/project-policy.md) — the project's overall policy (architecture, naming, Git workflow, the top-level §9 "Security Policy" section).
* [`privacy_policy.md`](./privacy_policy.md) — the privacy policy for users' personal data.
* [`threat_model.md`](./threat_model.md) — the STRIDE threat model and intruder model.
* [`business-logic.md`](../architecture/business-logic.md) — business logic, roles (`Guest`/`User`/`Admin`), recipe ownership rules.

## 12. Final Provisions

This policy takes effect once approved by the project owner. It is reviewed after significant changes to the architecture, technology stack, authentication/authorization mechanism, or development process, and after any item in Section 9 is remediated (the entry is marked resolved rather than deleted, to preserve history). Policy changes are recorded in a dedicated commit that bumps the document version. In case of conflict between this document and `project-policy.md`, the stricter constraint applies until the documents are reconciled.
