using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IReviewRepository : IRepository<Review>
{
	Task<Review> GetByUserAndRecipeId(Guid userId, Guid recipeId);
	Task<ICollection<Review>> GetAllByRecipeId(Guid recipeId);
}