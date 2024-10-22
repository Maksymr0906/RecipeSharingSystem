using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;

public partial class UserRoleConfiguration
	: IEntityTypeConfiguration<UserRole>
{
	public void Configure(EntityTypeBuilder<UserRole> builder)
	{
		builder.HasKey(r => new { r.UserId, r.RoleId });
	}
}