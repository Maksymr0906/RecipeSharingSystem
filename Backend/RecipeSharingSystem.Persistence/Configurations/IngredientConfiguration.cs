using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.SeedData;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class IngredientConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<Ingredient>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<Ingredient> builder)
	{
		builder.HasData(_seedDataOptions.Ingredients);
	}
}
