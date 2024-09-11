using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data.Repositories.Implementation
{
	public class RatingRepository : AbstractRepository<Rating>, IRatingRepository
	{
		public RatingRepository(RecipeSharingSystemDbContext context)
			: base(context)
		{
		}
	}
}
