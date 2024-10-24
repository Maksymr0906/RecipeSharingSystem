using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence;

public partial class UserRoleConfiguration(AuthorizationOptions authorization)
	: IEntityTypeConfiguration<UserRole>
{
	private readonly AuthorizationOptions _authorization = authorization;

	public void Configure(EntityTypeBuilder<UserRole> builder)
	{
		builder.HasKey(r => new { r.UserId, r.RoleId });
		builder.HasData(_authorization.UserRoles);
	}
}