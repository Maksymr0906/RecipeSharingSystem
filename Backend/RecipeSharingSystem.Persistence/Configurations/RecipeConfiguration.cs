using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.SeedData;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class RecipeConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<Recipe>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<Recipe> builder)
	{
		builder.HasMany(r => r.Categories)
			   .WithMany(c => c.Recipes)
			   .UsingEntity<Dictionary<string, object>>(
				   "RecipeCategory",
				   j => j.HasOne<Category>()
						 .WithMany()
						 .HasForeignKey("CategoryId")
						 .OnDelete(DeleteBehavior.Cascade),
				   j => j.HasOne<Recipe>()
						 .WithMany()
						 .HasForeignKey("RecipeId")
						 .OnDelete(DeleteBehavior.Cascade),
				   j =>
				   {
					   j.HasKey("RecipeId", "CategoryId");

					   j.HasData(
						   _seedDataOptions.RecipeCategories
					   );

				   });

		builder.HasData(_seedDataOptions.Recipes);
	}
}
