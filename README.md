# RecipeSharingSystem

Web application for sharing cooking recipes: ASP.NET Core 8 API (`Backend/`), Angular 16 SPA (`Frontend/`), MySQL.

## Configuration (required before the first run)

The API refuses to start without secrets. They are **not** stored in the repository: provide them with
[user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) (development) or environment variables (deployment).

Generate random values (PowerShell, works in Windows PowerShell 5.1 and PowerShell 7):

```powershell
function New-Secret { $b = New-Object byte[] 32; [Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($b); [Convert]::ToBase64String($b) }
```

Set them (run inside `Backend/RecipeSharingSystem.API`):

```powershell
dotnet user-secrets set "ConnectionStrings:RecipeSharingSystemConnectionString" "server=localhost;user=root;password=<db password>;database=recipe_sharing_system"
dotnet user-secrets set "JwtOptions:SecretKey" (New-Secret)
dotnet user-secrets set "Encryption:CurrentKeyId" "1"
dotnet user-secrets set "Encryption:Keys:1" (New-Secret)
```

Environment-variable equivalents use `__` instead of `:` (for example `JwtOptions__SecretKey`, `Encryption__Keys__1`).

| Setting | Purpose |
| --- | --- |
| `ConnectionStrings:RecipeSharingSystemConnectionString` | MySQL connection string including the password |
| `JwtOptions:SecretKey` | HMAC key that signs JWTs (at least 32 bytes) |
| `Encryption:CurrentKeyId`, `Encryption:Keys:<id>` | Key ring for encryption of personal data (Base64, at least 32 random bytes each) |
| `Cors:AllowedOrigins` (in `appsettings.json`) | Frontend origins allowed to call the API; default `http://localhost:4200` |
| `JwtOptions:Issuer`, `JwtOptions:Audience` (in `appsettings.json`, not secret) | Restrict accepted JWTs to tokens minted by and intended for this API — see [`docs/security/client_server_trust.md`](docs/security/client_server_trust.md) |

Never commit real values. If a secret was ever committed, treat it as compromised and replace it.

## Frontend

```powershell
cd Frontend/recipe-sharing-system
npm install
npm start
```

The SPA expects the API at `https://localhost:7130` (`src/environments/environment.development.ts`). The API must allow the SPA's origin in `Cors:AllowedOrigins` (default `http://localhost:4200`). Error answers of the API are shown by `ErrorInterceptor` (401 logs the user out and opens the login page; 400/403/409/429 show a message); pages for logged-in users are protected by `userGuard`.

## Database

Migrations are not stored in git (`.gitignore`). After pulling changes that touch the data model, recreate the development database:

```powershell
cd Backend
dotnet ef database drop --project RecipeSharingSystem.Persistence --startup-project RecipeSharingSystem.API
dotnet ef migrations add Initial --project RecipeSharingSystem.Persistence --startup-project RecipeSharingSystem.API
dotnet ef database update --project RecipeSharingSystem.Persistence --startup-project RecipeSharingSystem.API
```

Personal data columns (`FirstName`, `LastName`, `DateOfBirth`, `PostalCode`) are stored encrypted, including the demo seed users, using the key configured at the moment the migration is generated. Use the same `Encryption:Keys` when you apply the migration and run the API.

## Documentation

* `docs/policies/project-policy.md` — project policy
* `docs/architecture/business-logic.md` — roles and business rules
* `docs/security/` — security policy, privacy policy, threat model, identity and access, client-server trust model, cryptography, integrity and distribution, logging and monitoring, backup and recovery

## Release integrity (signing a build before deploying it)

See [`Backend/RecipeSharingSystem.ReleaseIntegrity/README.md`](Backend/RecipeSharingSystem.ReleaseIntegrity/README.md) and [`docs/security/integrity_and_distribution.md`](docs/security/integrity_and_distribution.md): a published release (API build / Angular production bundle) is hashed and signed with an RSA key the project owner keeps private, and `RecipeSharingSystem.ReleaseIntegrity verify` must pass before that release is deployed.

## Logging

The API writes structured logs to the console and to `Backend/RecipeSharingSystem.API/logs/` (rolling daily files, excluded from Git — see `.gitignore`). Configuration is under the `Serilog` section of `appsettings.json`. See [`docs/security/logging_and_monitoring.md`](docs/security/logging_and_monitoring.md) for what is logged and what is deliberately not (passwords, tokens, keys).

## Backups

`scripts/backup/Backup-Database.ps1` / `backup-db.sh` create a checksummed `mysqldump` of the database; `Restore-Database.ps1` / `restore-db.sh` verify that checksum before restoring. See [`docs/security/backup_and_recovery.md`](docs/security/backup_and_recovery.md) for the full strategy, retention policy, and a worked restore drill.
