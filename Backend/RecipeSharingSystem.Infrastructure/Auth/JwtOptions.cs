namespace RecipeSharingSystem.Infrastructure.Auth;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresHours { get; set; }

    /// <summary>
    /// Who issued the token (this API). Not a secret - safe to keep in appsettings.json.
    /// Checked by the token consumer (this same API) so a token minted for a different
    /// system cannot be replayed here, per lab 6's "restrict the token to a specific recipient".
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Who the token is intended for (this API's own clients). Not a secret.
    /// </summary>
    public string Audience { get; set; } = string.Empty;
}
