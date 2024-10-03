using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data.Repositories.Implementation
{
	public class RecipeRepository : AbstractRepository<Recipe>, IRecipeRepository
	{
		public RecipeRepository(RecipeSharingSystemDbContext context)
			: base(context)
		{
		}

		public async Task<ICollection<Recipe>> GetAllWithDetailsAsync()
		{
			var recipes = await Entities
				.Include(x => x.Categories)
				.Include(x => x.RecipeIngredients)
				.ThenInclude(x => x.Ingredient)
				.Include(x => x.Instruction)
				.Include(x => x.Ratings)
				.ToListAsync();

			return recipes;
		}

		public async Task<Recipe> GetWithDetailsByIdAsync(Guid id)
		{
			var recipe = await Entities
				.Include(x => x.Categories)
				.Include(x => x.RecipeIngredients)
				.ThenInclude(x => x.Ingredient)
				.Include(x => x.Instruction)
				.Include(x => x.Ratings)
				.FirstOrDefaultAsync(r => r.Id == id);
			if (recipe == null)
			{
				throw new KeyNotFoundException();
			}

			return recipe;
		}
	}
}
