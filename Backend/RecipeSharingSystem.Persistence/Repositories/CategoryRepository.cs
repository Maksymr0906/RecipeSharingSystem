using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class CategoryRepository(RecipeSharingSystemDbContext context)
	: Repository<Category>(context), ICategoryRepository
{
    // refactor
	public async Task<List<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> categoryIds)
    {
        if (categoryIds == null || !categoryIds.Any())
        {
            return new List<Category>();
        }

        return await Entities
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync();
    }

	public async Task<Category> GetCategoryBySlugAsync(string slug)
	{
        var category = await Entities.FirstOrDefaultAsync(c => c.Slug == slug);
        if (category == null)
        {
            throw new KeyNotFoundException();
        }

        return category;
	}
}
