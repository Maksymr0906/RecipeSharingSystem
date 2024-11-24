using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Infrastructure;

public interface IJwtProvider
{
	string Generate(User user);
}
