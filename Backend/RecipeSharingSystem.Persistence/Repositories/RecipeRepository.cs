using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class RecipeRepository(RecipeSharingSystemDbContext context)
	: Repository<Recipe>(context), IRecipeRepository
{
	public async Task<ICollection<Recipe>> GetAllWithDetailsAsync()
    {
		try
		{
			return await GetBaseRecipeQuery().ToListAsync();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error retrieving recipes: {ex.Message}", ex);
		}
    }

	public async Task<Recipe> GetBySlugAsync(string slug)
	{
		var recipe = await GetBaseRecipeQuery()
				.FirstOrDefaultAsync(r => r.Slug.ToLower().Trim() == slug.ToLower().Trim());

		if (recipe == null)
		{
			throw new KeyNotFoundException($"No recipe found with slug: {slug}");
		}

		return recipe;
	}

	public async Task<Recipe> GetWithDetailsByIdAsync(Guid id)
    {
		var recipe = await GetBaseRecipeQuery()
				.FirstOrDefaultAsync(r => r.Id == id);

		if (recipe == null)
		{
			throw new KeyNotFoundException($"No recipe found with ID: {id}");
		}

		return recipe;
    }

	public async Task<ICollection<Recipe>> SearchRecipesAsync(string[] searchTerms)
	{
		try
		{
			var query = GetBaseRecipeQuery();

			foreach (var term in searchTerms)
			{
				var searchTerm = term.ToLower();
				query = query.Where(r =>
					r.Title.ToLower().Contains(searchTerm) ||
					r.ShortDescription.ToLower().Contains(searchTerm) ||
					r.Instruction.Content.ToLower().Contains(searchTerm) ||
					r.RecipeIngredients.Any(ri =>
						ri.Ingredient.Name.ToLower().Contains(searchTerm)) ||
					r.Categories.Any(c => 
						c.Name.ToLower().Contains(searchTerm))
				);
			}

			var orderedResults = query.Select(r => new
				{
					Recipe = r,
					Priority = (
						r.Title.ToLower().Contains(searchTerms[0].ToLower()) ? 1 :
						r.ShortDescription.ToLower().Contains(searchTerms[0].ToLower()) ? 2 :
						r.Instruction.Content.ToLower().Contains(searchTerms[0].ToLower()) ? 3 :
						r.RecipeIngredients.Any(ri =>
							ri.Ingredient.Name.ToLower().Contains(searchTerms[0].ToLower())) ? 4 : 5
					)
				})
				.OrderBy(x => x.Priority)
				.Select(x => x.Recipe);

			return await orderedResults.ToListAsync();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error searching recipes: {ex.Message}", ex);
		}
	}

	private IQueryable<Recipe> GetBaseRecipeQuery()
	{
		return Entities
			.Include(x => x.Categories)
			.Include(x => x.RecipeIngredients)
				.ThenInclude(x => x.Ingredient)
			.Include(x => x.Instruction)
			.Include(x => x.Reviews)
			.Include(x => x.Author);
	}
}
