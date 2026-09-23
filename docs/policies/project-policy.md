# Project Policy — RecipeSharingSystem

|                    |                                      |
| ------------------ | ------------------------------------ |
| **Name**           | Project Policy — RecipeSharingSystem |
| **Version**        | 1.0                                |
| **Status**         | Draft                                |
| **Classification** | Internal                             |
| **Last Updated**   | 2026-09-23                           |
| **Owner**          | Project Developer                    |

> This policy defines the main requirements for the code, architecture, documentation, security, and development process of **RecipeSharingSystem**. The system is a web application designed to allow users to work with cooking recipes through a frontend application and a backend service.

> The project does not define business rules for external systems. Its scope is limited to recipe-sharing functionality, user interaction, data processing, and communication between the frontend and backend.

## 1. Policy Change Rules

* **No incidental changes** — this policy must not be changed as a side effect of an unrelated task.
* **Documented changes** — changes to the policy must be made intentionally and described in the corresponding commit.
* **Version increment** — significant changes to the policy require an increment of the document version.
* **Consistency** — the policy must remain consistent with the actual project structure and implementation.

## 2. Scope

This policy applies to the complete **RecipeSharingSystem** project, including:

* backend source code;
* frontend source code;
* data and database-related components;
* authentication and authorization mechanisms;
* validation and error handling;
* tests;
* documentation;
* Git repository and project configuration.

## 3. Technologies and Project Components

The project is organized as a web application with two primary parts:

* **Backend** — provides server-side application logic, data processing, API endpoints, validation, and access to persistent data.
* **Frontend** — provides the user interface and communicates with the backend.
* **Database** — stores persistent application data such as users, recipes, ingredients, and other entities required by the system.
* **Git/GitHub** — used for source-code version control and project collaboration.

Exact framework, language, library, and database versions must be taken from the project configuration files and must not be invented in documentation.

## 4. Fundamental Principles

* **Separation of frontend and backend.** UI logic must remain in the frontend, while server-side business and data-processing logic must remain in the backend.
* **Separation of responsibilities.** A component, class, function, or module should have one clearly defined responsibility.
* **Business logic belongs to the backend.** Important rules concerning recipes, users, permissions, validation, and data consistency must not be implemented only in the frontend.
* **Validation.** User-provided data must be validated before it is accepted or stored.
* **Security by default.** Authentication, authorization, input validation, protection of sensitive data, and safe error handling must be considered for every feature.
* **Consistent data model.** Changes to entities or relationships must be reflected in the database structure and ER documentation when applicable.
* **Clean code.** Code should be readable, maintainable, and avoid unnecessary duplication.
* **Explicit error handling.** Expected errors must be handled in a predictable way and must not expose internal implementation details.
* **Documentation follows the implementation.** Documentation must be updated when a feature or architectural decision changes.
* **No unnecessary complexity.** Architectural patterns should be introduced only when they solve an actual project requirement.

## 5. Architecture and Project Structure

The project follows a client-server structure:

```text
RecipeSharingSystem/
├── Backend/
│   └── RecipeSharingSystem.API/
│   └── RecipeSharingSystem.Application/
│   └── RecipeSharingSystem.Bussiness/
│   └── RecipeSharingSystem.Data/
│   └── RecipeSharingSystem.Infrastructure/
│   └── RecipeSharingSystem.Persistence/
├── Frontend/
│   └── recipe-sharing-system/
├── docs/
├── README.md
├── .gitignore
└── .gitattributes
```

The main communication flow is:

```text
User
  ↓
Frontend
  ↓
Backend API
  ↓
Business Logic
  ↓
Database
```

### 5.1 Frontend

The frontend is responsible for:

* displaying application pages and components;
* collecting user input;
* displaying recipes and other data received from the backend;
* sending requests to the backend;
* displaying validation and server errors;
* managing client-side UI state.

The frontend must not be treated as the only place where security-sensitive validation or business rules are enforced.

### 5.2 Backend

The backend is responsible for:

* exposing API endpoints;
* processing requests;
* validating input;
* applying business rules;
* authenticating users, where authentication is implemented;
* authorizing protected operations;
* reading and writing persistent data;
* returning appropriate responses and errors.

Business rules should be implemented in backend services or other dedicated components rather than duplicated across controllers.

### 5.3 Database and Persistence

The persistence layer is responsible for storing application data.

Database changes must preserve:

* entity relationships;
* data integrity;
* appropriate constraints;
* consistency between the database schema and backend models.

When a structural database change is introduced, the ER diagram or related documentation should be updated.

## 6. Naming Policy

### General

* Classes, interfaces, components, and types should use the naming convention required by the language and framework used by the project.
* Variables and functions should have descriptive names.
* Names should describe their purpose rather than their implementation details.
* Abbreviations should be avoided unless they are standard within the project.

### Backend

Examples:

```text
RecipeService
RecipeController
UserRepository
RecipeValidator
```

Names should clearly indicate the responsibility of the component.

### Frontend

Components should use names that describe the UI element or feature they represent.

Examples:

```text
recipe-card
recipe-list
recipe-details
login-form
```

## 7. API Policy

Backend API endpoints must:

* have clear and consistent names;
* use appropriate HTTP methods;
* validate incoming data;
* return predictable response structures;
* use appropriate HTTP status codes;
* avoid exposing sensitive internal information.

Typical operations may include:

```text
GET    /recipes
GET    /recipes/{id}
POST   /recipes
PUT    /recipes/{id}
DELETE /recipes/{id}
```

The actual endpoint names must follow the implemented API rather than this illustrative example.

## 8. Validation Policy

Validation must be performed on the backend for all data that can affect application state.

Examples include:

* required recipe fields;
* valid text length;
* valid identifiers;
* valid user credentials;
* valid relationships between entities;
* authorization requirements for modifying or deleting data.

Frontend validation may be used to improve user experience but does not replace backend validation.

## 9. Security Policy

Security must be considered in every feature that processes user-controlled data.

The project must:

* validate and sanitize input where appropriate;
* protect authenticated endpoints;
* verify user permissions before protected operations;
* avoid storing sensitive information in source code;
* keep credentials and secrets outside the repository;
* avoid returning passwords, tokens, or other sensitive information in ordinary API responses;
* avoid exposing database errors or internal stack traces to users;
* use parameterized database operations or the safe query mechanisms provided by the chosen data-access technology.

Secrets, passwords, API keys, and connection credentials must never be committed to Git.

## 10. Error Handling Policy

Errors must be handled explicitly.

The application should distinguish between:

* validation errors;
* authentication errors;
* authorization errors;
* resource-not-found errors;
* conflicts or invalid state;
* unexpected server errors.

User-facing errors should contain enough information to explain the problem without exposing internal implementation details.

## 11. Testing Policy

Testing should cover the most important application behavior.

Priority areas include:

* recipe creation and modification;
* recipe retrieval;
* recipe deletion;
* user registration and authentication, if implemented;
* authorization;
* input validation;
* API error handling;
* important frontend interactions.

Both successful and invalid scenarios should be considered.

Tests should not depend on real external services unless the test is explicitly intended to be an integration test.

## 12. Git Workflow

The repository uses Git for version control and GitHub for remote storage.

The main branch is:

```text
master
```

Changes should be committed in logical units.

Recommended commit format:

```text
[TYPE] short description
```

Examples:

```text
[INIT] add project documentation
[FEAT] add recipe creation
[FIX] validate recipe input
[DOCS] update business logic
```

Commits should:

* describe the actual change;
* avoid unrelated modifications;
* be reasonably small and understandable;
* not contain secrets or generated files that should be ignored.

## 13. Documentation Policy

Project documentation is stored in the `docs/` directory.

The following documents are recommended:

```text
docs/
├── project-policy.md
├── business-logic.md
├── requirements.md
├── architecture.md
├── testing.md
└── security.md
```

Documentation should describe the current state of the project.

When implementation changes an important business rule, architecture decision, or security requirement, the corresponding documentation must be updated.

## 14. Anti-Patterns

| #  | Anti-Pattern               | Description                                                                                      |
| -- | -------------------------- | ------------------------------------------------------------------------------------------------ |
| 1  | Business logic in UI       | Important business rules are implemented only in the frontend.                                   |
| 2  | Duplicate business logic   | The same business rule is independently implemented in multiple places.                          |
| 3  | Missing backend validation | The backend trusts data only because it was validated by the frontend.                           |
| 4  | Insecure data exposure     | API responses contain passwords, tokens, credentials, or unnecessary sensitive information.      |
| 5  | Hard-coded secrets         | Passwords, API keys, tokens, or database credentials are stored in source code.                  |
| 6  | God component              | One class/component handles unrelated responsibilities.                                          |
| 7  | Unclear naming             | Names do not describe the responsibility or purpose of the code.                                 |
| 8  | Silent errors              | Exceptions or failed operations are ignored without appropriate handling.                        |
| 9  | Documentation drift        | Documentation describes functionality or architecture that no longer matches the implementation. |
| 10 | Unrelated commits          | A commit mixes several unrelated changes, making review and rollback difficult.                  |
