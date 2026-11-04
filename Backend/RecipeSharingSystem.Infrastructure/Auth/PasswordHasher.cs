using RecipeSharingSystem.Core.Interfaces.Infrastructure;

namespace RecipeSharingSystem.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    // bcrypt cost factor (2^12 iterations). Review periodically: raise it as hardware gets faster.
    private const int WorkFactor = 12;

    // "Enhanced" mode pre-hashes the password with SHA-384 and Base64-encodes it before bcrypt,
    // so the 72-byte input limit of plain bcrypt does not silently truncate long passwords.
    private static readonly string DummyHash =
        BCrypt.Net.BCrypt.EnhancedHashPassword("timing-equalisation-dummy-password", workFactor: WorkFactor);

    public string Generate(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor: WorkFactor);

    public bool Verify(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);

    public void SimulateVerification(string password) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, DummyHash);
}
