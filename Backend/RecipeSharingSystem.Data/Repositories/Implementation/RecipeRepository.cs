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
	}
}
