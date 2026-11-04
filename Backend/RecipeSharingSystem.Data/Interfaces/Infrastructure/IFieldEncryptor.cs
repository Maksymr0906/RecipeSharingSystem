namespace RecipeSharingSystem.Core.Interfaces.Infrastructure;

/// <summary>
/// Authenticated encryption (AES-256-GCM) of individual sensitive values before they are stored.
/// </summary>
public interface IFieldEncryptor
{
	/// <param name="plaintext">Value to protect.</param>
	/// <param name="context">
	/// Associated data bound to the ciphertext (for example "User.PostalCode").
	/// The same value must be supplied to <see cref="Decrypt"/>, otherwise authentication fails.
	/// </param>
	string Encrypt(string plaintext, string context);

	/// <exception cref="System.Security.Cryptography.CryptographicException">
	/// The payload is malformed, was produced with an unknown key, was modified, or the context differs.
	/// </exception>
	string Decrypt(string payload, string context);
}
