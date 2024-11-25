using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class IngredientRepository(RecipeSharingSystemDbContext context)
	: Repository<Ingredient>(context), IIngredientRepository
{
    // refactor
	public async Task<Ingredient> GetByNameAsync(string name)
    {
        return await Entities.FirstOrDefaultAsync(ingredient => ingredient.Name == name);
    }

    // refactor
    public async Task<IEnumerable<Ingredient>> GetByNamesAsync(IEnumerable<string> names)
    {
        return await Entities
            .Where(ingredient => names.Contains(ingredient.Name))
            .ToListAsync();
    }

	public async Task<Ingredient> GetBySlugAsync(string slug)
	{
		var ingredient = await Entities.FirstOrDefaultAsync(c => c.Slug == slug);
        if (ingredient == null)
        {
            throw new KeyNotFoundException();
        }

        return ingredient;
	}
}
