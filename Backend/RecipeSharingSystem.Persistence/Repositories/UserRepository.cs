using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class UserRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<User>(context), IUserRepository
{
	public async Task<User> GetByEmail(string email)
	{
		var user = await Entities.FirstOrDefaultAsync(x => x.Email == email);
		return user;

	}
}
