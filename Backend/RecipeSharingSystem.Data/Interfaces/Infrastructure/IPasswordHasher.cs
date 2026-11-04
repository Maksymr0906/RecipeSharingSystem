namespace RecipeSharingSystem.Core.Interfaces.Infrastructure;

public interface IPasswordHasher
{
	string Generate(string password);
	bool Verify(string password, string passwordHash);

	/// <summary>
	/// Performs a verification against a fixed dummy hash so that a login attempt for an unknown
	/// account takes about as long as one for a known account.
	/// </summary>
	void SimulateVerification(string password);
}
