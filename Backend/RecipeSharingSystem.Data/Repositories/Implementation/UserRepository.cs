using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data.Repositories.Implementation
{
	public class UserRepository : AbstractRepository<User>, IUserRepository
	{
		public UserRepository(RecipeSharingSystemDbContext context)
			: base(context)
		{
		}
	}
}
