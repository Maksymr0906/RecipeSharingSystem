using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IReviewRepository : IRepository<Review>
{
	Task<ICollection<Review>> GetAllWithDetails();
	Task<Review> GetByIdWithDetails(Guid id);
	Task<Review> GetByUserAndRecipeId(Guid userId, Guid recipeId);
	Task<ICollection<Review>> GetAllByRecipeId(Guid recipeId);
}