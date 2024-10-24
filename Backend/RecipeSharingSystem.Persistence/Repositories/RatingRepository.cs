using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class RatingRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Rating>(context), IRatingRepository
{
}
