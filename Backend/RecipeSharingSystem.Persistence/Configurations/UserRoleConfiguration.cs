using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.SeedData;

public partial class UserRoleConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<UserRole>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<UserRole> builder)
	{
		builder.HasKey(r => new { r.UserId, r.RoleId });
		builder.HasData(_seedDataOptions.UserRoles);
	}
}