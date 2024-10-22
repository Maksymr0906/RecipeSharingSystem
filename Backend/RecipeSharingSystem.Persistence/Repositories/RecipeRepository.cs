using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class RecipeRepository(RecipeSharingSystemDbContext context)
    : AbstractRepository<Recipe>(context), IRecipeRepository
{
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
