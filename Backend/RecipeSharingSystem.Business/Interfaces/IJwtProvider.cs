using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Application.Interfaces;
public interface IJwtProvider
{
	string Generate(User user);
}
