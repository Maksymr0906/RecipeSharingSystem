# Security Policy — RecipeSharingSystem

| | |
| --- | --- |
| Title | Security Policy — RecipeSharingSystem |
| Version | 1.0.0 |
| Status | Draft |
| Classification | Internal |
| Last updated | 2026-09-25 |
| Owner | Maksym R. (project developer/owner) |
| Related documents | [`project-policy.md`](../policies/project-policy.md), [`privacy_policy.md`](./privacy_policy.md), [`threat_model.md`](./threat_model.md), [`business-logic.md`](../architecture/business-logic.md) |

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
* `Frontend/` — the Angular SPA;
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
* **Backups:** not currently implemented; out of scope for the lab assignment, but recorded as an open gap before any real deployment.

## 7. Secure Implementation

* **Input validation.** FluentValidation is used (`RegisterRequestDtoValidator`, `LoginRequestDtoValidator`, etc.). `UpdateUserRequestDtoValidator` currently has no rules at all — it must be filled in with constraints consistent with the `User` entity's annotations (`UserName` length ≤ 100, `Email` format/length ≤ 255, `FirstName`/`LastName` length ≤ 50, `PostalCode` length ≤ 20).
* **Authentication.** JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`); tokens are signed with HMAC-SHA256 using a symmetric key from configuration; lifetime and signature are validated. `ValidateIssuer`/`ValidateAudience` are currently disabled — acceptable only as long as tokens are both issued and consumed by this single API; if another service ever consumes the token, these checks must be enabled.
* **Authorization.** A custom permission-based system: the `Role → Permission` mapping is loaded from configuration and enforced by `PermissionAuthorizationHandler` via `[Authorize(Policy = "...")]`. **Mandatory requirement:** every endpoint that reads or modifies data belonging to a specific user, or performs a protected operation, must carry an explicit `[Authorize]` attribute; its absence is treated as a security defect (a concrete instance of this is recorded in Section 9).
* **Passwords.** Hashed with BCrypt (`EnhancedHashPassword`/`EnhancedVerify`) — an acceptable, modern algorithm. The current minimum length is 5 characters with no complexity requirement (`RegisterRequestDtoValidator`); raising the minimum to 8+ characters is recommended.
* **File uploads (`ImagesController`).** The file name (`fileName`) comes from the client and is used directly to build the server-side path via `Path.Combine` without sanitization, and the extension is taken from the uploaded file's original name without checking it against an allow-list. **Requirement:** `fileName` must reject path separators and `..` sequences (or be ignored entirely and generated server-side, e.g. as a GUID), and the extension/content must be validated against an explicit allow-list (`.jpg`, `.jpeg`, `.png`, `.webp`) with confirmation that the content is actually an image (e.g. by attempting to decode it with the already-referenced `SixLabors.ImageSharp`).
* **CORS.** `Program.cs` currently configures `AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()`. Acceptable for local development only; the policy must be restricted to the exact origin(s) of the deployed frontend before any public deployment.
* **Error handling.** Controllers explicitly catch `ValidationException` and return `BadRequest`. For unhandled exceptions, it must be confirmed that detailed error pages/stack traces are disabled outside the Development environment (`app.Environment.IsDevelopment()` is already used for Swagger — the same approach should be applied to error handling).
* **Logging.** Passwords, password hashes, and JWT tokens must never be written to application logs.

## 8. Security Verification

* **Automated tests.** The repository currently has no dedicated test project — per `project-policy.md` §11, adding unit/integration tests covering authentication, authorization (including the ownership rule and admin override from `business-logic.md` §7), and input validation is recommended.
* **Code review.** With a single developer, self-review before each commit is required against a short checklist: no new secrets in code/configuration; every endpoint that reads or modifies a specific user's data has `[Authorize]`; no ownership check has been removed or bypassed.
* **Dependency scanning.** Run `dotnet list package --vulnerable` and `npm audit` before every release/lab submission.
* **Static analysis.** Built-in Roslyn analyzers and nullable-reference warnings are used (`<Nullable>enable</Nullable>` in every `.csproj`) — new compiler warnings must not be ignored.
* **CI:** open item — add a GitHub Actions workflow to automatically build the backend (`dotnet build`) and frontend (`ng build`) on every push/PR.
* **Release gate.** A change must not land on `master` if it: adds a secret in plaintext; removes an existing `[Authorize]` attribute; or removes an ownership check on a resource.

## 9. Vulnerability Management

**Process:** register → classify by severity (Critical / High / Medium / Low) → assign an owner (the repository owner) → remediate → re-verify → update this document, `threat_model.md`, or `business-logic.md` if the issue reveals a gap in them.

**Register compiled while preparing this policy** (verified against the actual repository code as of 2026-09-25):

| # | Finding | Severity | Component | Description / recommendation |
| - | ------- | :------: | --------- | ------------------------------ |
| V1 | The DB password and JWT signing key are committed in plaintext | **Critical** | `Backend/RecipeSharingSystem.API/appsettings.json` | Move both to environment variables / user-secrets; after removing them from source, both values must be rotated, since the old ones must be treated as compromised. |
| V2 | `UsersController.GetUserById` and `UpdateUser` have no `[Authorize]` attribute | **Critical** | `Backend/RecipeSharingSystem.API/Controllers/UsersController.cs` | Any caller — even unauthenticated — can read or modify any user's profile by GUID: a classic Broken Access Control / IDOR issue. Add `[Authorize]` and a check that `id` matches the caller's own id (or that the caller is an Admin). |
| V3 | `ImagesController.CreateImage` builds the file path from a client-supplied `fileName` without sanitization | High | `Backend/RecipeSharingSystem.API/Controllers/ImagesController.cs` | Potential path traversal / write outside the intended directory. Sanitize or generate the name server-side; validate the extension/content against an allow-list. |
| V4 | The CORS policy allows any origin (`AllowAnyOrigin`) | Medium | `Backend/RecipeSharingSystem.API/Program.cs` | Acceptable for local development only; restrict before deployment. |
| V5 | Minimum password length is 5 characters with no complexity rule | Low | `RegisterRequestDtoValidator.cs` | Raise the minimum (recommended ≥ 8) per §7. |

These items are also the input to `threat_model.md`, where each one is tied to specific STRIDE threats (the `V1`–`V5` labels in the threat model correspond to this table).

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