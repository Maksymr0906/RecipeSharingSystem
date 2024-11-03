using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.SeedData;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class RecipeIngredientConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<RecipeIngredient>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
	{
		builder.HasKey(ri => new { ri.RecipeId, ri.IngredientId });

		builder.HasData(_seedDataOptions.RecipeIngredients);
	}
}
