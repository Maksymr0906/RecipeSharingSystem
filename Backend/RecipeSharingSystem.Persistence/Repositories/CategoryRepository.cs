﻿using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class CategoryRepository(RecipeSharingSystemDbContext context)
	: Repository<Category>(context), ICategoryRepository
{
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
        return await Entities.
            FirstOrDefaultAsync(c => c.Slug == slug);
	}
}
