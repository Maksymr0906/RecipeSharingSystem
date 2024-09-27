using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Data.Repositories.Interfaces
{
	public interface IRecipeRepository : IAbstractRepository<Recipe>
	{
		Task<Recipe> GetRecipeWithDetailsById(Guid id);
	}
}
