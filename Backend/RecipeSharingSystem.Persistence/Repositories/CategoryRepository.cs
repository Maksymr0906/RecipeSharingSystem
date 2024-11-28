using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class CategoryRepository(RecipeSharingSystemDbContext context)
	: Repository<Category>(context), ICategoryRepository
{
	public async Task<List<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> categoryIds)
    {
		if (categoryIds == null)
		{
			throw new ArgumentNullException(nameof(categoryIds), "Category IDs cannot be null.");
		}

		if (!categoryIds.Any())
        {
            return new List<Category>();
        }

        try
        {
			return await Entities
				.Where(c => categoryIds.Contains(c.Id))
				.ToListAsync();
		}
        catch(Exception ex)
        {
			throw new InvalidOperationException($"Error retrieving categories: {ex.Message}", ex);
		}
    }

	public async Task<Category> GetCategoryBySlugAsync(string slug)
	{
        var category = await Entities.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == slug);

        if (category == null)
        {
            throw new KeyNotFoundException();
        }

        return category;
	}
}
