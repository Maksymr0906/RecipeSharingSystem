using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories
{
    public class UserRepository : AbstractRepository<User>, IUserRepository
    {
        public UserRepository(RecipeSharingSystemDbContext context)
            : base(context)
        {
        }
    }
}
