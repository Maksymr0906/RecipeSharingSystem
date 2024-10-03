using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Data.Repositories.Interfaces
{
	public interface IRecipeRepository : IAbstractRepository<Recipe>
	{
		Task<Recipe> GetWithDetailsByIdAsync(Guid id);
		Task<ICollection<Recipe>> GetAllWithDetailsAsync();
	}
}
