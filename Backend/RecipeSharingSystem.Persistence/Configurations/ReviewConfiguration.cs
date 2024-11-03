using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.SeedData;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class ReviewConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<Review>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.HasIndex(r => new { r.UserId, r.RecipeId })
			   .IsUnique();

		builder.HasOne(r => r.User)
			   .WithMany(u => u.Reviews)
			   .HasForeignKey(r => r.UserId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(r => r.Recipe)
			   .WithMany(r => r.Reviews)
			   .HasForeignKey(r => r.RecipeId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.HasData(_seedDataOptions.Reviews);
	}
}
