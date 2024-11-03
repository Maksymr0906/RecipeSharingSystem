using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.SeedData;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class ImageConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<Image>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<Image> builder)
	{
		builder.HasData(_seedDataOptions.Images);
	}
}
