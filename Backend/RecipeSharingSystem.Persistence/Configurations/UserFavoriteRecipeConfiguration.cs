using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.SeedData;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class UserFavoriteRecipeConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<UserFavoriteRecipe>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<UserFavoriteRecipe> builder)
	{
		builder.HasKey(uf => new { uf.UserId, uf.RecipeId });

		builder.HasData(_seedDataOptions.UserFavoriteRecipes);
	}
}
