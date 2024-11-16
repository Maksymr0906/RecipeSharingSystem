﻿using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class RecipeRepository(RecipeSharingSystemDbContext context)
	: Repository<Recipe>(context), IRecipeRepository
{
	public async Task<ICollection<Recipe>> GetAllWithDetailsAsync()
    {
        var recipes = await Entities
            .Include(x => x.Categories)
            .Include(x => x.RecipeIngredients)
            .ThenInclude(x => x.Ingredient)
            .Include(x => x.Instruction)
            .Include(x => x.Reviews)
			.Include(x => x.Author)
			.ToListAsync();

        return recipes;
    }

	public async Task<Recipe> GetBySlugAsync(string slug)
	{
		return await Entities
			.Include(x => x.Categories)
			.Include(x => x.RecipeIngredients)
			.ThenInclude(x => x.Ingredient)
			.Include(x => x.Instruction)
			.Include(x => x.Reviews)
            .Include(x => x.Author)
			.FirstOrDefaultAsync(r => r.Slug == slug);
	}

	public async Task<Recipe> GetWithDetailsByIdAsync(Guid id)
    {
        var recipe = await Entities
            .Include(x => x.Categories)
            .Include(x => x.RecipeIngredients)
            .ThenInclude(x => x.Ingredient)
            .Include(x => x.Instruction)
			.Include(x => x.Reviews)
			.Include(x => x.Author)
			.FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
        {
            throw new KeyNotFoundException();
        }

        return recipe;
    }
}
