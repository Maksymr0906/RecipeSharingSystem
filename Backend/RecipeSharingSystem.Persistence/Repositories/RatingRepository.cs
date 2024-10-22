using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class RatingRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Rating>(context), IRatingRepository
{
}
