# Release integrity tool

Signs and verifies a release artifact (the published API, the production
Angular bundle, or any other folder) so that its recipient can tell whether
it is exactly what the developer published, and not a tampered copy.

Design rationale, and why a plain checksum or a shared-secret MAC were not
enough, are in [`docs/security/integrity_and_distribution.md`](../../docs/security/integrity_and_distribution.md).

## One-time setup (developer only)

Generate an RSA key pair. The **private key never leaves your machine and is
never committed** (see the repository's `.gitignore`); only the public key
is checked in, at `release-signing/public_key.pem`.

```powershell
# PowerShell, OpenSSL (ships with Git for Windows)
openssl genpkey -algorithm RSA -pkeyopt rsa_keygen_bits:3072 -out private_key.pem
openssl rsa -in private_key.pem -pubout -out RecipeSharingSystem.ReleaseIntegrity/release-signing/public_key.pem
```

Store `private_key.pem` somewhere safe outside the repository (a password
manager, a CI secret). If it is ever lost or exposed, generate a new pair and
replace `public_key.pem` - every release signed with the old key will then
need to be re-signed.

## Signing a release

```powershell
dotnet run --project Backend/RecipeSharingSystem.ReleaseIntegrity -- sign `
  --dir   <folder with the published build, e.g. API/bin/Release/net8.0/publish> `
  --key   <path to your private_key.pem> `
  --out   <folder to write manifest.sha256 + manifest.sha256.sig into>
```

Ship the release folder together with `manifest.sha256` and
`manifest.sha256.sig` (e.g. as extra files in the same GitHub release / zip).

## Verifying a release

```powershell
dotnet run --project Backend/RecipeSharingSystem.ReleaseIntegrity -- verify `
  --dir      <folder with the downloaded release> `
  --manifest <folder containing manifest.sha256 and manifest.sha256.sig> `
  --pubkey   RecipeSharingSystem.ReleaseIntegrity/release-signing/public_key.pem
```

Exit code `0` and `OK: ...` means every file matches the signed manifest and
the signature itself is valid for the committed public key. Anything else
(exit code `1`) means the release must **not** be deployed or installed:
a wrong hash, an extra undeclared file, or a signature that does not verify
(wrong key, or the manifest itself was edited) are all treated as failures.
