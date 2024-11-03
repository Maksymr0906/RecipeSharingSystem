using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.SeedData;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class RatingConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<Rating>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<Rating> builder)
	{
		builder.HasIndex(r => new { r.UserId, r.RecipeId })
			   .IsUnique();

		builder.HasOne(r => r.User)
			   .WithMany(u => u.Ratings)
			   .HasForeignKey(r => r.UserId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(r => r.Recipe)
			   .WithMany(r => r.Ratings)
			   .HasForeignKey(r => r.RecipeId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.HasData(_seedDataOptions.Ratings);
	}
}
