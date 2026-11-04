# Cryptographic Protection — RecipeSharingSystem

| | |
| --- | --- |
| Version | 1.0.0 |
| Related | [`privacy_policy.md`](./privacy_policy.md), [`identity_and_access.md`](./identity_and_access.md) |

## 1. Sensitive data inventory

| Data | Where | Protection |
| --- | --- | --- |
| Password | `Users.PasswordHash` | bcrypt, cost 12, salted (one-way) |
| First name, last name, date of birth, postal code | `Users` columns | **AES-256-GCM field encryption** in the application |
| E-mail | `Users.Email` | Plain text: it is the login key and must be searchable/unique |
| JWT signing key, DB password, encryption keys | user-secrets / environment variables | Not in the repository |
| Data in transit | browser ↔ API | HTTPS (TLS), configured at deployment |

## 2. How and where encryption happens

* Implemented in `AesGcmFieldEncryptor` (Infrastructure) behind `IFieldEncryptor`; applied transparently through EF Core value converters in `UserConfiguration` (encrypt on write, decrypt on read). The database and its backups only see ciphertext.
* Algorithm: **AES-256-GCM** (authenticated encryption). Lesson from lab 4: unauthenticated CTR is malleable and a reused keystream is recoverable from known plaintext (see the experiment), so an AEAD mode is used.
* Per value: random 12-byte nonce, 16-byte tag; stored as `enc.{keyId}.{Base64(nonce | ciphertext | tag)}`.
* The column name (e.g. `User.PostalCode`) is bound as associated data, so a ciphertext copied into another column fails authentication.
* A data key is derived from each master key with **HKDF-SHA256** and a fixed purpose string, so the master key can later serve other purposes without key reuse.

## 3. Key management

* Master keys (≥ 32 random bytes, Base64) are provided as `Encryption:Keys:<id>`; `Encryption:CurrentKeyId` selects the key for new writes. Old keys stay in the ring to decrypt old rows. Never in Git.
* The application checks the key ring at startup and refuses to start if it is missing or malformed.
* **Rotation:** add a new key id, switch `CurrentKeyId`, run `dotnet run -- --reencrypt-pii` (re-encrypts all rows with the current key), then retire the old key once no row refers to it. Rotate immediately on suspected compromise.
* Back up the keys separately from database backups; losing a key means losing the data it protects.

## 4. Limitations

* Associated data is the column name, not the row id, so ciphertext can be swapped between rows of the same column.
* Encrypted columns cannot be searched or sorted in SQL; this is why e-mail is not encrypted.
* Decryption is strict: rows written before encryption was introduced are not readable — recreate the database or migrate them first.
* Seed (demo) users are encrypted with the key present when the migration is generated; the migration therefore changes whenever the key does.
* Keys are in application configuration, so an attacker who controls the application host gets both the keys and the data. A KMS / HSM would remove this.
