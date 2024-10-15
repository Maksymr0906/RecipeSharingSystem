using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories
{
    public class RatingRepository : AbstractRepository<Rating>, IRatingRepository
    {
        public RatingRepository(RecipeSharingSystemDbContext context)
            : base(context)
        {
        }
    }
}
