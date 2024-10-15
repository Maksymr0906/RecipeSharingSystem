using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Core.Repositories;

public interface IRecipeRepository : IAbstractRepository<Recipe>
{
	Task<Recipe> GetWithDetailsByIdAsync(Guid id);
	Task<ICollection<Recipe>> GetAllWithDetailsAsync();
}
