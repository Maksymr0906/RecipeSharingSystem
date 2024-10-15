using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class UserRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<User>(context), IUserRepository
{
}
