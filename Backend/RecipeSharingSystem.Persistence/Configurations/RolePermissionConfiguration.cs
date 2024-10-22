using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class RolePermissionConfiguration
	: IEntityTypeConfiguration<RolePermission>
{
	private readonly AuthorizationOptions _authorization;

	public RolePermissionConfiguration(AuthorizationOptions authorization)
	{
		_authorization = authorization;
	}

	public void Configure(EntityTypeBuilder<RolePermission> builder)
	{
		builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

		builder.HasData(ParseRolePermissions());
	}

	private RolePermission[] ParseRolePermissions()
	{
		return _authorization.RolePermissions
			.SelectMany(rp => rp.Permissions
				.Select(p => new RolePermission
				{
					RoleId = (int)Enum.Parse<RoleEnum>(rp.Role),
					PermissionId = (int)Enum.Parse<PermissionEnum>(p)
				}))
				.ToArray();

	}
}
