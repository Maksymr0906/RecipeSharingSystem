using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class UserConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<User>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasData(_seedDataOptions.Users);
	}
}
