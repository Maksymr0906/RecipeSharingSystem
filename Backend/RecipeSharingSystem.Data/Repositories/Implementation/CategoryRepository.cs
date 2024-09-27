using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data.Repositories.Implementation
{
	public class CategoryRepository : AbstractRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(RecipeSharingSystemDbContext context) 
			: base(context)
		{
		}

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
}
