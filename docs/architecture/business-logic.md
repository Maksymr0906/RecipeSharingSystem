# Business Logic — RecipeSharingSystem

## 1. Purpose

**RecipeSharingSystem** is a web application for sharing and managing cooking recipes.

The system provides a frontend for user interaction and a backend responsible for processing requests, applying business rules, and working with persistent data.

The main business domain is the management and sharing of recipes and the users associated with them.

## 2. Main Actors

### 2.1 Guest User

A guest is a visitor who has not authenticated in the system.

Depending on the implemented functionality, a guest may:

* view publicly available recipes;
* view recipe details;
* browse available recipe information;
* access authentication-related pages.

A guest must not perform operations that require an authenticated account.

### 2.2 Registered User

A registered user is an authenticated account holder with the standard role.

A registered user may:

* manage their own account;
* create recipes;
* edit recipes they own;
* delete recipes they own;
* view recipes;
* interact with the recipe-sharing functionality provided by the system.

A registered user must not manage other users' accounts or moderate content that does not belong to them.

### 2.3 Administrator

An administrator is an authenticated account holder with elevated (system-wide) permissions, on top of everything a registered user may do.

An administrator may:

* view, edit, and delete **any** recipe in the system, regardless of ownership;
* view the list of registered users;
* view any user's account details;
* change a user's role (e.g. promote a user to administrator, or revoke admin rights);
* disable, ban, or delete a user account;
* moderate content (e.g. remove recipes that violate platform rules);
* access administrative reports or system-wide data views, if implemented.

An administrator's elevated permissions apply system-wide and are not limited by the ownership rule described in Section 7.

Only an existing administrator (or a system-level bootstrap/seed process) may grant the administrator role to another account. A registered user must not be able to self-promote to administrator through any client-supplied field.

## 3. Core Business Entities

The central business entity of the system is the **Recipe**.

Other entities may include users and recipe-related data such as ingredients, categories, or other entities represented by the project's database model.

The repository contains an ER diagram that documents relationships between persistent entities.

### 3.1 User

A User represents an account that interacts with the RecipeSharingSystem.

Typical responsibilities include:

* identifying the account;
* storing account-related information;
* participating in authentication;
* owning or being associated with recipes;
* holding a **role** (`Guest` is not persisted as an account; persisted accounts hold `User` or `Admin`) that determines permissions for protected operations.

A user must be uniquely identifiable by the system.

A user's role must be stored and enforced on the backend; it must never be trusted from a value the frontend sends on a request.

### 3.2 Recipe

A Recipe represents a cooking recipe shared through the application.

A recipe contains the information required to describe how a dish can be prepared.

Typical recipe information may include:

* title;
* description;
* ingredients;
* preparation instructions;
* author/owner;
* other attributes defined by the database model.

A recipe must have sufficient valid information to be displayed and used by the application.

### 3.3 Recipe-Related Entities

If the implementation contains separate entities for ingredients, categories, comments, ratings, or other recipe-related information, these entities represent supporting parts of the recipe-sharing domain.

Their relationships must follow the database model and must not contradict the business rules of the system.

## 4. Recipe Lifecycle

A recipe normally follows this lifecycle:

```text
Create
  ↓
Validate
  ↓
Store
  ↓
View
  ↓
Edit
  ↓
Store updated version
  ↓
Delete (when permitted)
```

### 4.1 Recipe Creation

When an authenticated user (registered user or administrator) creates a recipe:

1. The frontend collects recipe information.
2. The frontend sends the data to the backend.
3. The backend validates the received data.
4. The backend verifies that the user is allowed to create a recipe.
5. The recipe is created and stored, associated with the creating user as owner.
6. The backend returns the result to the frontend.
7. The frontend displays the updated state.

Invalid data must not create a recipe.

### 4.2 Recipe Viewing

When a user requests a recipe:

1. The frontend requests recipe data from the backend.
2. The backend identifies the requested recipe.
3. The backend checks whether the recipe can be accessed.
4. The backend retrieves the required data.
5. The backend returns the recipe information.
6. The frontend displays the recipe.

If the recipe does not exist, the backend must return an appropriate not-found response.

### 4.3 Recipe Editing

When a user edits a recipe:

1. The frontend sends the updated information.
2. The backend validates the data.
3. The backend identifies the existing recipe.
4. The backend verifies that the current user has permission to modify it: either they are the **owner**, or they are an **administrator**.
5. The recipe is updated.
6. The updated result is returned to the frontend.

A registered user must not be able to modify another user's recipe. An administrator may modify any recipe.

### 4.4 Recipe Deletion

When a user requests deletion:

1. The backend identifies the recipe.
2. The backend verifies that the recipe exists.
3. The backend verifies that the user has permission to delete it: either they are the **owner**, or they are an **administrator**.
4. The recipe is deleted or marked as deleted according to the implemented persistence model.
5. The backend returns the operation result.

Unauthorized deletion must be rejected.

### 4.5 User Management (Administrator only)

When an administrator manages a user account:

1. The frontend sends the target user identifier and the requested action (e.g. change role, disable, delete).
2. The backend validates the request data.
3. The backend verifies that the requesting account is an administrator.
4. The backend verifies the target user exists.
5. The backend applies the action (role change, disable/ban, or delete).
6. The backend returns the operation result.

A non-administrator must never be able to perform user-management actions, even against their own account, through this flow.

## 5. Validation Rules

The system must validate recipe and user-management data before persistence.

Validation should include, where applicable:

* required fields must be present;
* text fields must satisfy length restrictions;
* identifiers must have a valid format;
* referenced entities must exist;
* data must satisfy the constraints defined by the database model;
* user permissions and role must be checked for protected operations;
* role-change requests must specify a valid, supported role.

Validation rules should be enforced by the backend even if equivalent validation exists in the frontend.

## 6. Authentication and Authorization

Authentication determines whether a user is logged in and identifies the account making a request.

Authorization determines whether the identified user's **role** allows a particular operation.

The general rule is:

```text
Authentication → identify user
Authorization  → verify role/permission (User vs Admin)
Operation      → execute business action
```

Operations that modify protected resources must not rely only on information supplied by the frontend, including the caller's claimed role.

The backend must determine the identity and role of the requesting user from server-side session/token data, not from request body fields.

## 7. Ownership Rules

Where recipes are associated with their creator, the creator is considered the owner of the recipe.

Ownership is important for protected operations such as:

* editing;
* deleting;
* managing private information associated with the recipe.

The frontend may hide controls that a user cannot use, but the backend must enforce the ownership rule independently.

**Administrator override:** an administrator is not bound by the ownership rule and may perform owner-level operations (edit, delete) on any recipe. This override exists for moderation and support purposes and must be applied only to accounts whose role is verified as `Admin` on the backend.

## 8. Error Scenarios

The business logic must account for invalid and exceptional situations.

Important scenarios include:

| Situation                              | Expected behavior                                         |
| --------------------------------------- | ----------------------------------------------------------|
| Invalid recipe data                     | Reject the request and return a validation error          |
| Missing required data                   | Reject the request                                         |
| Recipe does not exist                   | Return a not-found response                                |
| Unauthenticated protected request       | Reject the request                                          |
| User lacks permission                   | Return an authorization error                                |
| Non-admin attempts admin-only action    | Return an authorization error                                 |
| Target user does not exist (admin flow) | Return a not-found response                                    |
| Invalid referenced entity               | Reject the request                                               |
| Database/persistence failure            | Return a controlled server error                                  |
| Unexpected internal error               | Log internally and avoid exposing implementation details          |

## 9. Business Invariants

The following invariants must be preserved:

* A recipe must contain valid required information.
* A recipe must have a valid association with its owner when ownership is part of the implemented model.
* A user cannot modify protected resources without the required permission (ownership or admin role).
* A user cannot delete protected resources without the required permission (ownership or admin role).
* Only an administrator can perform user-management actions (role changes, disabling, deleting accounts).
* A user cannot grant themselves the administrator role.
* Invalid references must not be stored.
* Backend business rules must not depend on frontend validation, including the frontend's claim about the caller's role.
* Persistent data must remain consistent with the defined relationships.

## 10. Frontend and Backend Responsibilities

### Frontend

The frontend is responsible for:

* presenting the user interface, including role-appropriate views (e.g. an admin panel shown only to administrators);
* collecting input;
* providing client-side validation for usability;
* sending API requests;
* displaying successful results;
* displaying errors returned by the backend.

### Backend

The backend is responsible for:

* authentication;
* authorization, including role checks (`User` vs `Admin`);
* validation;
* business rules, including the administrator override on ownership;
* data consistency;
* persistence;
* API responses;
* security-sensitive operations.

The frontend must not be considered a trusted environment. In particular, showing or hiding an admin panel in the UI is not a substitute for backend authorization checks.

## 11. Business Logic Boundaries

The following responsibilities belong to the RecipeSharingSystem:

* managing users within the application's own domain, including roles;
* managing recipes;
* validating recipe-related and user-management data;
* controlling access to protected recipe and user-management operations;
* storing and retrieving application data;
* providing the API used by the frontend.

The following should not be implemented as business logic in the frontend:

* final authorization decisions;
* trusted ownership or role checks;
* database operations;
* security-sensitive validation;
* generation or storage of server-side secrets.

## 12. Main Business Flow

The general recipe-sharing flow is:

```text
User
  │
  ├── Register / Authenticate
  │
  ↓
Frontend
  │
  ├── Create Recipe
  ├── View Recipe
  ├── Edit Recipe
  ├── Delete Recipe
  └── (Admin only) Manage Users / Moderate Recipes
  │
  ↓
Backend API
  │
  ├── Authenticate
  ├── Authorize (role: User / Admin)
  ├── Validate
  ├── Apply Business Rules (incl. admin override)
  └── Persist Data
  │
  ↓
Database
```

## 13. Explicit Non-Goals

The following are outside the core business logic unless they are explicitly introduced as project requirements:

* financial transactions;
* food delivery;
* restaurant management;
* inventory management;
* external marketplace operations;
* business rules belonging to third-party systems.

External integrations, if added later, must have their own documented responsibilities and interfaces.

## 14. Consistency With the Implementation

This document describes the business-level behavior of RecipeSharingSystem.

When implementation details change, this document must be updated if the change affects:

* a business rule;
* an entity;
* an ownership or role rule;
* authentication or authorization;
* a recipe lifecycle;
* validation requirements;
* a core user interaction.

Technical implementation details that do not change business behavior should be documented in the appropriate technical documentation instead.