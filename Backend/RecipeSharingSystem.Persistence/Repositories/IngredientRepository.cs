using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class IngredientRepository(RecipeSharingSystemDbContext context)
    : Repository<Ingredient>(context), IIngredientRepository
{
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
