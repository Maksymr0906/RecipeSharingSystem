# Software Integrity and Distribution Protection — RecipeSharingSystem

| | |
| --- | --- |
| Version | 1.0.0 |
| Related | [`security_policy.md`](./security_policy.md) §6 "Software Protection", [`cryptography.md`](./cryptography.md) |

## 1. What already has integrity protection, and what is new here

Two kinds of data in this project were already protected against undetected
modification by earlier lab work, for reasons specific to their own
lifecycle, not because of a project-wide strategy:

* **JWTs** carry an HMAC-SHA256 signature (`JwtOptions:SecretKey`): the API
  rejects a token whose claims (user id, roles) were altered after issuing.
* **Encrypted personal data** (`FirstName`, `LastName`, `DateOfBirth`,
  `PostalCode`) use AES-256-**GCM**, an authenticated mode: a modified
  ciphertext fails to decrypt instead of silently producing garbage.

Day-to-day application data that a logged-in user is allowed to change
(recipes, reviews, instructions, the user's own profile fields) is
intentionally **mutable**, and the risk there is *unauthorized* writing, not
*undetected* corruption; it is already addressed by authentication,
ownership checks and FluentValidation (`docs/security/identity_and_access.md`).
A cryptographic integrity check on data that is supposed to change on every
legitimate request would not add protection.

**What was missing, and is the subject of this lab:** nothing verified that
a *published release* (the compiled API output, or the production Angular
bundle) is exactly what the project owner actually built and published,
as opposed to a copy altered somewhere between the build machine and
whoever deploys or downloads it (a compromised CI runner, a mirror, a
man-in-the-middle on a plain-HTTP download). This is the "software
artifact" sense of integrity from the lab's theoretical material (MDC / MAC
/ digital signature), and it is the gap this document and the
`RecipeSharingSystem.ReleaseIntegrity` tool close.

## 2. Why a plain checksum, and why not a shared-secret MAC

* **A bare SHA-256 checksum (MDC)** shipped next to the release proves
  nothing: whoever tampers with the release can recompute a new, matching
  checksum. A checksum only helps when the *reference value* reaches the
  recipient through a channel the attacker does not control — which is
  exactly the problem a signature avoids needing.
* **An HMAC-based MAC** would need the verification secret to live wherever
  the release is checked (a deployment server, or a self-hoster's machine)
  — i.e., in an environment outside the project owner's control. Whoever
  reads that secret out could then re-sign a tampered release, and a single
  leaked secret would break trust for every past and future release.
* **An asymmetric digital signature** keeps the one value that matters
  secret (the private key) on the machine that produces releases, and
  distributes only the public key, which cannot be used to forge a new
  signature. This is the scheme used by essentially all real package/update
  signing (code signing certificates, APT/Homebrew release signing, etc.),
  and it is what this project implements.

## 3. Decision

* **Algorithm:** SHA-256 for per-file hashes (the MDC layer), RSA-3072
  **PSS** with SHA-256 (not the older PKCS#1 v1.5 padding) for the
  signature, matching the scheme the lab material recommends for new
  systems.
* **What gets signed:** a manifest listing the relative path and SHA-256
  hash of every file in the release folder (e.g. `dotnet publish` output,
  or `ng build --configuration production`'s `dist/` folder), built and
  signed by `RecipeSharingSystem.ReleaseIntegrity sign`.
* **Key storage:** the private key is generated once by the project owner
  and never committed (`.gitignore` enforces this); only
  `RecipeSharingSystem.ReleaseIntegrity/release-signing/public_key.pem` is
  tracked in the repository, so anyone who clones the repository can verify
  a release without ever being able to produce a new valid one.
* **Verification is mandatory before deployment, not advisory:** the tool
  returns a non-zero exit code on any mismatch — a missing file, an extra
  undeclared file, a changed hash, or a signature that does not verify — so
  a deployment script can simply refuse to proceed when `verify` fails,
  exactly as the lab material requires for update mechanisms ("якщо
  перевірка не пройдена, оновлення повинно бути відхилене").
* **Where the check runs:** as a release-pipeline / deployment-time step
  (`RecipeSharingSystem.ReleaseIntegrity verify`), not as a self-check
  inside the running API. A web service verifying its own already-running
  binary adds little here: the project owner already controls the
  production host directly (see §4, the chosen distribution model), so the
  realistic point of tampering is the handoff between "built by the owner"
  and "running on a server", which is exactly what signing the release
  closes.

## 4. Distribution model

RecipeSharingSystem is published as a **source-available, self-hosted web
application** (GitHub repository, MIT-style use expected): there is no sale,
no per-seat license key, and no executable installed on an end user's
machine to crack — end users only ever interact with the deployed web
service through a browser. Functionally this already matches the **SaaS**
category from the lab's theory: all business rules and all enforcement
(authentication, ownership checks, rate limiting, field encryption) run
**server-side**; the Angular client holds no secret and no logic whose
bypass would grant extra access, because every privileged decision is
re-checked by the API regardless of what the client sends (see
`identity_and_access.md`).

Because of this, classic anti-piracy mechanisms (Trialware/Crippleware-style
feature locks, hardware-bound license files, obfuscating a client-side
license check) do not apply and are deliberately **not** implemented: there
is no paid tier or seat count to protect, and the project is meant to be
freely cloned and self-hosted. The actual way this distribution model gets
"violated" is a **tampered build** reaching a deployment (whether the
owner's own production server or another person's self-hosted instance)
without anyone noticing — which is precisely the threat §3 defends against.
If the project ever adds a paid/hosted tier in the future, licensing would
be enforced the same way the rest of the project enforces authorization:
as a server-side check against the database, never as a client-side secret.

## 5. Residual risks

* Signing only proves *what the owner published*; it says nothing about
  whether the owner's own build machine was compromised before signing.
* The current tool does not timestamp or log which key signed which
  release; for a single-developer student project a single key pair is
  proportionate, but a team would want release metadata (version, commit
  hash) inside the signed manifest and a process for key rotation and
  revocation.
* Loss of the private key does not expose past releases, but makes new
  releases unverifiable with the committed public key until it is replaced.
