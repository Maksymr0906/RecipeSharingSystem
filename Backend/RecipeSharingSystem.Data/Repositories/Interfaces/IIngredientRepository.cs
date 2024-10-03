using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Data.Repositories.Interfaces
{
	public interface IIngredientRepository : IAbstractRepository<Ingredient>
	{
		Task<IEnumerable<Ingredient>> GetByNamesAsync(IEnumerable<string> names);
		Task<Ingredient> GetByNameAsync(string name);
	}
}
