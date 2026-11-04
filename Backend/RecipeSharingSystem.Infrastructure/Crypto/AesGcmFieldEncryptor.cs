using Microsoft.Extensions.Options;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;
using System.Security.Cryptography;
using System.Text;

namespace RecipeSharingSystem.Infrastructure.Crypto;

/// <summary>
/// AES-256-GCM field encryption.
///
/// Stored format:  enc.{keyId}.{Base64(nonce[12] | ciphertext | tag[16])}
///
/// * A purpose-specific data key is derived from each master key with HKDF-SHA256, so the master key
///   itself is never used to encrypt data and other purposes (for example TOTP secrets) can get their own keys.
/// * A fresh random 96-bit nonce is generated for every encryption.
/// * The "context" string is passed as associated data, so a value copied into another column fails authentication.
/// * The key identifier is stored with the value, which makes key rotation possible without downtime.
/// </summary>
public class AesGcmFieldEncryptor : IFieldEncryptor
{
	private const string Prefix = "enc";
	private const int NonceSize = 12;
	private const int TagSize = 16;
	private const int KeySize = 32;
	private const int MinimumMasterKeySize = 32;

	private static readonly byte[] HkdfInfo =
		Encoding.UTF8.GetBytes("RecipeSharingSystem|pii-field-encryption|v1");

	private readonly string _currentKeyId;
	private readonly Dictionary<string, byte[]> _dataKeys = new();

	public AesGcmFieldEncryptor(IOptions<EncryptionOptions> options)
	{
		var settings = options.Value;

		if (settings.Keys.Count == 0)
		{
			throw new InvalidOperationException(
				"No encryption keys configured. Set Encryption:CurrentKeyId and Encryption:Keys:<id> " +
				"(Base64 of 32 random bytes) through user-secrets or environment variables.");
		}

		foreach (var (keyId, base64) in settings.Keys)
		{
			if (string.IsNullOrWhiteSpace(keyId) || keyId.Contains('.'))
			{
				throw new InvalidOperationException("Encryption key identifiers must be non-empty and must not contain '.'.");
			}

			byte[] masterKey;
			try
			{
				masterKey = Convert.FromBase64String(base64);
			}
			catch (FormatException)
			{
				throw new InvalidOperationException($"Encryption key '{keyId}' is not valid Base64.");
			}

			if (masterKey.Length < MinimumMasterKeySize)
			{
				throw new InvalidOperationException($"Encryption key '{keyId}' must be at least {MinimumMasterKeySize} bytes.");
			}

			_dataKeys[keyId] = HKDF.DeriveKey(HashAlgorithmName.SHA256, masterKey, KeySize, null, HkdfInfo);
		}

		if (!_dataKeys.ContainsKey(settings.CurrentKeyId))
		{
			throw new InvalidOperationException($"Encryption:CurrentKeyId '{settings.CurrentKeyId}' is not present in Encryption:Keys.");
		}

		_currentKeyId = settings.CurrentKeyId;
	}

	public string Encrypt(string plaintext, string context)
	{
		var plainBytes = Encoding.UTF8.GetBytes(plaintext);
		var nonce = RandomNumberGenerator.GetBytes(NonceSize);
		var cipherBytes = new byte[plainBytes.Length];
		var tag = new byte[TagSize];

		using (var aes = new AesGcm(_dataKeys[_currentKeyId], TagSize))
		{
			aes.Encrypt(nonce, plainBytes, cipherBytes, tag, Encoding.UTF8.GetBytes(context));
		}

		var blob = new byte[NonceSize + cipherBytes.Length + TagSize];
		Buffer.BlockCopy(nonce, 0, blob, 0, NonceSize);
		Buffer.BlockCopy(cipherBytes, 0, blob, NonceSize, cipherBytes.Length);
		Buffer.BlockCopy(tag, 0, blob, NonceSize + cipherBytes.Length, TagSize);

		return $"{Prefix}.{_currentKeyId}.{Convert.ToBase64String(blob)}";
	}

	public string Decrypt(string payload, string context)
	{
		var parts = payload.Split('.');

		if (parts.Length != 3 || parts[0] != Prefix || !_dataKeys.TryGetValue(parts[1], out var dataKey))
		{
			throw new CryptographicException("Encrypted value has an unknown format or was produced with an unknown key.");
		}

		byte[] blob;
		try
		{
			blob = Convert.FromBase64String(parts[2]);
		}
		catch (FormatException)
		{
			throw new CryptographicException("Encrypted value is not valid Base64.");
		}

		if (blob.Length < NonceSize + TagSize)
		{
			throw new CryptographicException("Encrypted value is too short.");
		}

		var cipherLength = blob.Length - NonceSize - TagSize;
		var nonce = blob.AsSpan(0, NonceSize);
		var cipherBytes = blob.AsSpan(NonceSize, cipherLength);
		var tag = blob.AsSpan(NonceSize + cipherLength, TagSize);
		var plainBytes = new byte[cipherLength];

		using (var aes = new AesGcm(dataKey, TagSize))
		{
			// Throws if the tag does not match; no plaintext is returned in that case.
			aes.Decrypt(nonce, cipherBytes, tag, plainBytes, Encoding.UTF8.GetBytes(context));
		}

		return Encoding.UTF8.GetString(plainBytes);
	}
}
