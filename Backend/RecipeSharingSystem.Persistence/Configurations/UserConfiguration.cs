using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;
using RecipeSharingSystem.Persistence.SeedData;
using System.Globalization;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class UserConfiguration(SeedDataOptions seedDataOptions, IFieldEncryptor fieldEncryptor)
	: IEntityTypeConfiguration<User>
{
	private const string DateFormat = "yyyy-MM-dd";

	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;
	private readonly IFieldEncryptor _fieldEncryptor = fieldEncryptor;

	public void Configure(EntityTypeBuilder<User> builder)
	{
		// Identifiers must be unique (case-insensitive with the default MySQL collation).
		builder.HasIndex(u => u.Email).IsUnique();
		builder.HasIndex(u => u.UserName).IsUnique();

		// Personal data is encrypted before it is written and decrypted when it is read.
		// Ciphertext is longer than the plaintext, hence the wider columns; the real limits
		// (50 / 50 / 20 characters) are enforced by UpdateUserRequestDtoValidator.
		builder.Property(u => u.FirstName)
			.HasMaxLength(512)
			.HasConversion(EncryptedString("User.FirstName"));

		builder.Property(u => u.LastName)
			.HasMaxLength(512)
			.HasConversion(EncryptedString("User.LastName"));

		builder.Property(u => u.PostalCode)
			.HasMaxLength(512)
			.HasConversion(EncryptedString("User.PostalCode"));

		builder.Property(u => u.DateOfBirth)
			.HasMaxLength(512)
			.HasConversion(EncryptedDate("User.DateOfBirth"));

		builder.HasData(_seedDataOptions.Users);
	}

	private ValueConverter<string, string> EncryptedString(string context) =>
		new(
			plain => _fieldEncryptor.Encrypt(plain, context),
			stored => _fieldEncryptor.Decrypt(stored, context));

	private ValueConverter<DateTime, string> EncryptedDate(string context) =>
		new(
			date => _fieldEncryptor.Encrypt(date.ToString(DateFormat, CultureInfo.InvariantCulture), context),
			stored => DateTime.ParseExact(_fieldEncryptor.Decrypt(stored, context), DateFormat, CultureInfo.InvariantCulture));
}
