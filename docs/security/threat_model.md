# Threat Model — RecipeSharingSystem

| | |
| --- | --- |
| Title | Threat Model — RecipeSharingSystem |
| Version | 1.0.0 |
| Status | Draft |
| Last updated | 2026-09-25 |
| Owner | Maksym R. (project developer/owner) |
| Related documents | [`security_policy.md`](./security_policy.md), [`privacy_policy.md`](./privacy_policy.md), [`business-logic.md`](../architecture/business-logic.md) |

## Scope

This model covers the entire RecipeSharingSystem application within a single deployment: the Angular SPA (client), the ASP.NET Core Web API (backend), the MySQL database, and the local image file storage for recipes/categories.

Out of scope: protection of the host operating system, physical access to hardware, and the production deployment's network infrastructure (the system is not currently deployed publicly) — these will be covered by a separate model once a deployment approach is chosen.

A single document covers the whole project, since the system is small (one API project, one database, one client) and splitting it into per-module models would not add value at this stage.

## Intruder Model

| Intruder | Access level | Capabilities |
| --- | --- | --- |
| **Anonymous network user (Guest)** | No account, network access to the public API only | Can call any endpoint not protected by `[Authorize]`; in the Development environment, can view the full API specification via Swagger. |
| **Registered user (User)** | Valid JWT with the `User` role | Everything a Guest can do, plus: create/edit/delete their own recipes, ingredients, instructions, and reviews; manage their own favorites list. |
| **Malicious / compromised User** | Valid JWT with standard `User` permissions | May attempt to exploit missing authorization checks (e.g., in `UsersController`) or the image-upload vulnerability to affect data or files outside their own scope. |
| **Administrator (Admin), including compromised or malicious** | Valid JWT with the `Admin` role | Full override of the ownership rule (`business-logic.md` §7): can edit/delete any recipe, view and change any user's role, and disable/delete accounts. Compromise of an admin account is the highest-impact incident. |
| **Insider with repository or database server access** | Direct access to Git history, the `appsettings.json` file, or the MySQL server itself | Can read the DB password and JWT signing key directly from configuration (see V1 below), connect to the database bypassing the API entirely, or forge a valid JWT offline given knowledge of the signing key. |

## System Overview

RecipeSharingSystem is a client-server web application for publishing and browsing recipes.

* **Client (Angular SPA)** runs in the user's browser and talks to the backend over a REST API (JSON, and `multipart/form-data` for image uploads); it holds onto the issued JWT for subsequent requests (the client uses `jwt-decode`, typical of reading a token from storage accessible to JS code).
* **Web API (ASP.NET Core 8)** handles authentication (issues JWTs), authorization (permission-based policies), the business logic for recipes/users/reviews, reads and writes data in MySQL, and accepts/serves images from a local `Images/` directory.
* **MySQL Database** stores all persistent data: users, roles/permissions, recipes, ingredients, instructions, reviews, favorites.
* **Local Image Storage** — the `Images/` directory on the API server's file system, publicly readable via `/Images/*`.

**System elements:**

| ID | DFD Type | Name |
| --- | --- | --- |
| E1 | External Interactor | User (Guest / Registered User / Administrator) |
| P1 | Process | Angular SPA (Client, runs in the user's browser) |
| P2 | Process | ASP.NET Core Web API (`RecipeSharingSystem.API`) |
| D1 | Data Store | MySQL Database (`recipe_sharing_system`) |
| D2 | Data Store | Local image file storage (`Images/`) |
| F1 | Data Flow | Client → Web API (HTTPS REST/JSON, `multipart/form-data` for images) |
| F2 | Data Flow | Web API → MySQL Database (EF Core / Pomelo.MySql) |
| F3 | Data Flow | Web API → File storage (read/write of image files) |
| TB1 | Trust Boundary | Internet / user's browser ↔ Web API |
| TB2 | Trust Boundary | Web API process ↔ database and file storage |

**Relationships:**

* `E1 User` interacts with `P1 Angular SPA` through the browser.
* `P1 Angular SPA` sends requests to `P2 Web API` via `F1 Client → Web API`.
* `F1` crosses `TB1 Internet ↔ Web API` — the boundary between the untrusted client and the trusted server.
* `P2 Web API` communicates with `D1 MySQL Database` via `F2`.
* `P2 Web API` reads/writes files in `D2 Local Image Storage` via `F3`.
* `F2` and `F3` cross `TB2` — the boundary between the API process and the data stores, which each have their own credentials/access rights.

## STRIDE Analysis

Canonical mapping of STRIDE categories to DFD element types:

| DFD Element | S | T | R | I | D | E |
| --- | :---: | :---: | :---: | :---: | :---: | :---: |
| External Interactor | ✓ | | ✓ | | | |
| Data Flow | | ✓ | | ✓ | ✓ | |
| Data Store | | ✓ | | ✓ | ✓ | |
| Process | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ |

Legend: **S** — Spoofing; **T** — Tampering; **R** — Repudiation; **I** — Information Disclosure; **D** — Denial of Service; **E** — Elevation of Privilege.

The labels `[V1]`–`[V5]` reference the findings register in `security_policy.md` §9, where severity and remediation guidance are recorded.

| Element | S | T | R | I | D | E |
| --- | --- | --- | --- | --- | --- | --- |
| **E1 User** *(External Interactor)* | An attacker can authenticate impersonating another user by guessing or compromising an email/password pair; the minimum password length is only 5 characters with no complexity requirement `[V5]`, which makes guessing easier. | — | Without a centralized audit log of actions (who/when/what operation), a user can deny having performed an action, e.g. deleting a recipe or changing a profile. | — | — | — |
| **F1 Client → Web API** *(Data Flow)* | — | The request could be modified by a man-in-the-middle if a production environment does not guarantee correct TLS (`UseHttpsRedirection()` is present, but certificate termination on a real host is not described, since the system is not deployed). | — | The open CORS policy (`AllowAnyOrigin`) `[V4]` lets any site running in the user's browser send requests carrying the JWT, if the token is accessible to client-side script; combined with a potential XSS in the SPA, this raises the risk of session hijacking. | No endpoint has rate limiting — brute-forcing `/api/Auth/login`, or flooding the API with requests to exhaust resources, is possible. | — |
| **D1 MySQL Database** *(Data Store)* | — | If the database credentials — which, as established, are stored in plaintext in the repository `[V1]` — are compromised, an attacker can directly modify data (recipes, user roles in the `UserRoles` table) bypassing every API check. | — | The same access exposes all users' personal data (`Email`, `PasswordHash`, `DateOfBirth`, `PostalCode`) with no interaction with the API at all. | A direct connection to the database allows deleting or corrupting data, breaking the availability of the whole service. | — |
| **D2 Local Image Storage** *(Data Store)* | — | The unsanitized `fileName` parameter of `POST /api/Images` `[V3]` lets an authenticated user with `Create` permission supply a name containing path-traversal sequences (e.g. `..` segments), potentially writing a file outside the intended `Images/` directory. | — | Files under `Images/` are intentionally public (served without authentication by design, to display recipe images); the risk arises only if a non-image file ends up there via `[V3]` — such a file would then also be publicly accessible. | There is no limit on upload size or count per user — an attacker could exhaust server disk space via mass uploads of large files. | — |
| **P1 Angular SPA** *(Process)* | Spoofing the client application itself is unlikely (served from a trusted origin), but a successful XSS would let an attacker's script act as the legitimate user's session in their browser. | If content rendered as Markdown/HTML (`marked`/`ngx-markdown`, e.g. for recipe instructions or reviews) is not sufficiently sanitized, an XSS injection could alter displayed data or perform actions as the user. | Client-side code can be modified via the browser's DevTools, so client-side checks alone are never proof of who actually initiated an action — logging must happen server-side only. | If the JWT is kept in storage accessible to JS code (typical for clients using `jwt-decode` on the frontend), a successful XSS leads to token theft and subsequent impersonation. | No SPA-specific denial-of-service threats were identified beyond generic ones (mass heavy requests to the API). | If the client ever relied on its own role check to unlock admin functionality, this would allow privilege escalation; under the current architecture (`business-logic.md` §6, §10) the server always re-checks permissions — this invariant must be preserved going forward. |
| **P2 ASP.NET Core Web API** *(Process)* | `[V2]` `UsersController.GetUserById` and `UpdateUser` have no `[Authorize]` attribute — any call, including an unauthenticated one, can act as if it owned an arbitrary account, reading or modifying its data by GUID. | `[V3]` Path traversal via the `fileName` parameter in `ImagesController.CreateImage` (see D2). Direct role changes via the API are not possible (the profile-update DTO has no role field) — this invariant should be preserved. | There is no centralized audit logging of critical operations (recipe deletion, an admin changing a role) — it is impossible to conclusively establish who performed an action in a dispute. | `[V2]` Unauthorized `GET /api/Users/{id}` exposes `Email`, `FirstName`, `LastName`, `DateOfBirth`, `PostalCode` of any user; user GUIDs (e.g. a recipe's `AuthorId`) are publicly visible via other endpoints, which simplifies enumeration. | No endpoint has rate limiting — potential overload via mass requests (e.g. to `GetRandomRecipes` or `SearchRecipes`). | `[V2]` Because `UpdateUser` is unprotected, any caller can theoretically edit another user's profile; direct escalation to the `Admin` role through this path is not possible (the DTO has no role field), which limits but does not eliminate the impact. |

> Where a single underlying issue produces several different STRIDE threats, they are recorded in each relevant cell (for example, `V2` is simultaneously Spoofing, Information Disclosure, and partially Elevation of Privilege for `P2`). STRIDE categories are not mutually exclusive and are used as a systematic way to search for threats rather than a strict classification.

## Traceability to the Vulnerability Register

| ID | Finding (`security_policy.md` §9) | STRIDE categories in this model |
| --- | --- | --- |
| V1 | Secrets (DB password, JWT key) committed in plaintext in `appsettings.json` | Tampering, Information Disclosure — `D1 MySQL Database` |
| V2 | Missing `[Authorize]` on `UsersController.GetUserById`/`UpdateUser` | Spoofing, Information Disclosure, Elevation of Privilege — `P2 Web API` |
| V3 | Unsanitized `fileName` in `ImagesController.CreateImage` | Tampering — `D2 Local Image Storage`, `P2 Web API` |
| V4 | CORS allows any origin | Information Disclosure — `F1 Client → Web API` |
| V5 | Minimum password length is 5 characters | Spoofing — `E1 User` |

This table provides two-way traceability: every entry in the security policy's vulnerability register has a corresponding rationale in the threat model, and, conversely, every concrete (non-hypothetical) threat in this model traces back to a register entry that must be remediated.