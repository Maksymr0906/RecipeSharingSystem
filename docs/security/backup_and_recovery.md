# Backup and disaster recovery (lab 7-8)

This covers the database backup strategy for RecipeSharingSystem, written after the course's Libraxis
incident scenario ended with the attacker issuing `DROP DATABASE catalog1` through a SQL injection and
every client of the system immediately starting to fail with `Unknown database 'catalog1'`. The
strategy and scripts below exist so that, if the same thing happened here, there is a tested, scripted
way back - not an improvised one.

## What needs to be backed up

| Asset | Backed up how | Why |
|---|---|---|
| MySQL database (`RecipeSharingSystem`) | Logical dump (`mysqldump`), see below | The only genuinely irreplaceable asset - user accounts, recipes, reviews, encrypted PII fields. |
| Application code, EF Core migrations, configuration templates | Git (GitHub) | Already versioned; a fresh server is rebuilt from a `git clone` + `dotnet publish`, not from a backup. |
| AES field-encryption key ring (`Encryption:Keys` in secrets) | Stored in the secrets manager / user-secrets, *not* in the database backup | A dump without the matching key ring is undecryptable ciphertext; the key ring and the data it protects must be backed up and rotated through separate channels (see `cryptography.md`) so that a single stolen or lost backup copy does not also hand over the keys. |
| Release signing key (lab 5) | Out of scope here - see `integrity_and_distribution.md` | Unrelated asset, already documented separately. |

Uploaded recipe images (`RecipeSharingSystem.API/Images/`) are not covered by the scripts below; if this
project were deployed for real, they would need their own backup (e.g. periodic sync to object storage)
since they are not inside the database.

## Strategy

- **Type:** full logical backup (`mysqldump --single-transaction`), not a filesystem/physical copy -
  simpler to verify and restore selectively, and consistent with a small-scale student project. A
  larger production deployment would add binary-log shipping for point-in-time recovery between full
  backups; that is out of scope here but noted as the natural next step.
- **Frequency:** daily, plus an ad-hoc run before any risky operation (a migration, the `--reencrypt-pii`
  key rotation, a manual data fix). Scheduled via `cron` (Linux) or Windows Task Scheduler running
  `scripts/backup/backup-db.sh` / `Backup-Database.ps1`.
- **Retention:** 30 days of daily backups kept locally, consistent with the 90-day application-log
  retention in `logging_and_monitoring.md`; a longer-term (monthly, kept 12 months) copy is recommended
  if the organisation's data-retention policy requires it, but is not implemented here.
- **Storage location (3-2-1 rule):** the backup directory (`db-backups/`) must not be the only copy and
  must not live on the same disk/VM as the database server - at minimum, copy it to a second host; ideally
  also to offsite/cloud storage. The scripts write locally and print a reminder to copy the file onward;
  automating that second copy (e.g. `aws s3 cp`, `rclone`) is an infrastructure decision left to the
  actual deployment environment, intentionally not hard-coded here.
- **Integrity verification:** a SHA-256 checksum is computed immediately after every dump and stored
  alongside it (`<dump>.sha256`). `Restore-Database.ps1`/`restore-db.sh` recompute and compare this hash
  **before** touching the database, and refuse to restore on a mismatch - demonstrated below.
- **Confidentiality of the backup file itself:** the dump contains plaintext for every column that is
  *not* application-level-encrypted (emails, recipe text, etc. - the PII fields covered by
  `cryptography.md` are already ciphertext inside the dump). Both scripts accept an optional passphrase
  to encrypt the dump at rest with `openssl enc -aes-256-cbc -pbkdf2` and delete the plaintext copy, so a
  stolen backup file alone is not a stolen database.
- **RPO (Recovery Point Objective):** with daily backups, up to 24 hours of writes can be lost in the
  worst case (a disaster right before the next scheduled backup). If this is unacceptable, increase
  backup frequency or add binary-log-based point-in-time recovery.
- **RTO (Recovery Time Objective):** measured directly in the drill below: from "database is gone" to
  "data fully restored and row counts verified" took a few seconds end-to-end for this project's data
  volume. Real production RTO additionally includes locating the right backup, getting operator access,
  and verifying application health - budget for minutes, not seconds, and re-measure at production scale.
- **Testing:** an untested backup is not a backup. The restore procedure below should be re-run
  periodically (e.g. monthly) against a disposable database, not assumed to still work because it worked
  once.

## Restore drill (actually executed, not simulated)

To verify the whole chain end-to-end, a representative schema (Users/Roles/Categories/Recipes/Reviews,
modelled on this project's entities but simplified - not the exact production schema, since no local
MySQL instance with the real EF Core migrations applied was available while writing this) was created
in a disposable MySQL instance and seeded with sample data:

```
mysql> SELECT (SELECT COUNT(*) FROM Users) AS users, (SELECT COUNT(*) FROM Recipes) AS recipes,
       (SELECT COUNT(*) FROM Reviews) AS reviews, (SELECT COUNT(*) FROM Categories) AS categories;
+-------+---------+---------+------------+
| users | recipes | reviews | categories |
+-------+---------+---------+------------+
|     5 |      10 |      20 |          3 |
+-------+---------+---------+------------+
```

1. **Backup**, using the same `mysqldump --single-transaction --no-tablespaces --routines --triggers`
   command as `scripts/backup/backup-db.sh`:
   ```
   $ mysqldump ... --databases recipesharing_demo > recipesharing_demo_20261121_194236.sql
   $ sha256sum recipesharing_demo_20261121_194236.sql
   30d6a74c05719ea1f8250e15a6141bd2ac1cb700ae7b5b0b1c1322660b949b11
   ```
2. **Simulated disaster** - the exact Libraxis scenario, reproduced for real:
   ```
   $ mysql -e "DROP DATABASE recipesharing_demo;"
   $ mysql -e "USE recipesharing_demo;"
   ERROR 1049 (42000): Unknown database 'recipesharing_demo'
   ```
   (This is the identical error class seen in the Libraxis application log, `SQLSTATE[HY000] [1049]
   Unknown database 'catalog1'`.)
3. **Tamper-rejection check**, confirming the safety mechanism before trusting it with a real restore:
   one byte was appended to a copy of the dump, its checksum recomputed, and compared against the
   original `.sha256` file:
   ```
   Expected: 30d6a74c05719ea1f8250e15a6141bd2ac1cb700ae7b5b0b1c1322660b949b11
   Actual (tampered): 62659baccb15e33236a386c513faa94bc170ea45f67f124169f9afc62f8d6828
   Checksum mismatch correctly detected -> restore would be aborted.
   ```
4. **Restore**, with the real (untampered) backup, checksum verified first:
   ```
   Expected: 30d6a74c05719ea1f8250e15a6141bd2ac1cb700ae7b5b0b1c1322660b949b11
   Actual:   30d6a74c05719ea1f8250e15a6141bd2ac1cb700ae7b5b0b1c1322660b949b11
   Checksum verified OK.
   $ mysql < recipesharing_demo_20261121_194236.sql
   ```
5. **Verification** - row counts after restore, compared against step 1:
   ```
   +-------+---------+---------+------------+
   | users | recipes | reviews | categories |
   +-------+---------+---------+------------+
   |     5 |      10 |      20 |          3 |
   +-------+---------+---------+------------+
   ```
   Identical to the pre-disaster counts - the restore is confirmed complete.

[TO ADD: скриншот аналогічного прогону (backup → симуляція втрати → restore → перевірка) на вашій реальній базі `RecipeSharingSystem`, після того як застосуєте зміни і матимете доступ до `dotnet`/MySQL локально.]

## Related documents

- [`logging_and_monitoring.md`](./logging_and_monitoring.md) - how the loss would first be noticed
  (error spike, `AuthorizationDenied`/`5xx` monitoring) before a restore is even triggered.
- [`cryptography.md`](./cryptography.md) - the field-encryption key ring that a restored database still
  depends on.
- [`security_policy.md`](./security_policy.md) - vulnerability register.
