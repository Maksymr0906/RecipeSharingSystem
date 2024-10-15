using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class RatingRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Rating>(context), IRatingRepository
{
}
