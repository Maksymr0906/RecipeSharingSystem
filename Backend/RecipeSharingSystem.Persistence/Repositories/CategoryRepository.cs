using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class CategoryRepository(RecipeSharingSystemDbContext context)
    : AbstractRepository<Category>(context), ICategoryRepository
{
	public async Task<List<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> categoryIds)
    {
        if (categoryIds == null || !categoryIds.Any())
        {
            return new List<Category>();
        }

        return await Context.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync();
    }
}
