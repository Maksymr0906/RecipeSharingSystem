using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IRatingRepository : IRepository<Rating>
{
	Task<Rating> GetByUserAndRecipeId(Guid userId, Guid recipeId);
}
