using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data.Repositories.Implementation
{
	public class IngredientRepository : AbstractRepository<Ingredient>, IIngredientRepository
	{
		public IngredientRepository(RecipeSharingSystemDbContext context)
			: base(context)
		{
		}

		public async Task<Ingredient> GetByNameAsync(string name)
		{
			return await Entities.FirstOrDefaultAsync(ingredient => ingredient.Name == name);
		}

		public async Task<IEnumerable<Ingredient>> GetByNamesAsync(IEnumerable<string> names)
		{
			return await Entities
				.Where(ingredient => names.Contains(ingredient.Name))
				.ToListAsync();
		}
	}
}
