using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class IngredientRepository(RecipeSharingSystemDbContext context)
	: Repository<Ingredient>(context), IIngredientRepository
{
	public async Task<Ingredient> GetByNameAsync(string name)
    {
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Ingredient name cannot be null or empty.", nameof(name));
		}

		var ingredient = await Entities
				.AsNoTracking()
				.FirstOrDefaultAsync(i =>
					i.Name.ToLower().Trim() == name.ToLower().Trim());

		if (ingredient == null)
		{
			throw new KeyNotFoundException($"No ingredient found with name: {name}");
		}

		return ingredient;
	}

	public async Task<Ingredient> GetBySlugAsync(string slug)
	{
		var ingredient = await Entities.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == slug);

        if (ingredient == null)
        {
            throw new KeyNotFoundException();
        }

        return ingredient;
	}
}
