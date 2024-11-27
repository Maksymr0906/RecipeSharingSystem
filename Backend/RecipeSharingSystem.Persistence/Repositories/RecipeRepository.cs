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
		var normalizedSearchTerms = searchTerms
			.Where(term => !string.IsNullOrWhiteSpace(term))
			.Select(term => term.ToLower().Trim())
			.Distinct()
			.ToArray();

		try
		{
			var baseQuery = GetBaseRecipeQuery();
			var query = baseQuery.AsQueryable();

			foreach (var term in normalizedSearchTerms)
			{
				query = query.Where(r =>
					r.Title.ToLower().Contains(term) ||
					r.ShortDescription.ToLower().Contains(term) ||
					r.Instruction.Content.ToLower().Contains(term) ||
					r.RecipeIngredients.Any(ri =>
						ri.Ingredient.Name.ToLower().Contains(term)) ||
					r.Categories.Any(c =>
						c.Name.ToLower().Contains(term))
				);
			}

			var orderedResults = query
					.Select(r => new
					{
						Recipe = r,
						Priority = CalculateSearchPriority(r, normalizedSearchTerms[0])
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
			.AsNoTracking()
			.Include(x => x.Categories)
			.Include(x => x.RecipeIngredients)
				.ThenInclude(x => x.Ingredient)
			.Include(x => x.Instruction)
			.Include(x => x.Reviews)
			.Include(x => x.Author);
	}

	private int CalculateSearchPriority(Recipe recipe, string primarySearchTerm)
	{
		if (recipe.Title.ToLower().Contains(primarySearchTerm))
			return 1;
		if (recipe.ShortDescription.ToLower().Contains(primarySearchTerm))
			return 2;
		if (recipe.Instruction.Content.ToLower().Contains(primarySearchTerm))
			return 3;
		if (recipe.RecipeIngredients.Any(ri =>
			ri.Ingredient.Name.ToLower().Contains(primarySearchTerm)))
			return 4;
		if (recipe.Categories.Any(c =>
			c.Name.ToLower().Contains(primarySearchTerm)))
			return 5;

		return 6;
	}
}
