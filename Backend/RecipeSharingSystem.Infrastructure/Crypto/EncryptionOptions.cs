namespace RecipeSharingSystem.Infrastructure.Crypto;

/// <summary>
/// Key ring for field encryption. Bound from the "Encryption" configuration section.
/// Master keys must come from user-secrets or environment variables, never from a committed file.
/// </summary>
public class EncryptionOptions
{
	/// <summary>Identifier of the key used for all new encryptions.</summary>
	public string CurrentKeyId { get; set; } = string.Empty;

	/// <summary>
	/// Master keys by identifier; each value is Base64 of at least 32 random bytes.
	/// Retired keys stay here until every stored value has been re-encrypted with the current key.
	/// </summary>
	public Dictionary<string, string> Keys { get; set; } = new();
}
