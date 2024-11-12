using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.SeedData;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class CategoryConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<Category>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.HasIndex(c => c.Slug).IsUnique();

		builder.HasData(_seedDataOptions.Categories);
	}
}
