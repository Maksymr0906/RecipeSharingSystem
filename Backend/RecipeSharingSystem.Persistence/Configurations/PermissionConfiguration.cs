using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class PermissionConfiguration
	: IEntityTypeConfiguration<Permission>
{
	public void Configure(EntityTypeBuilder<Permission> builder)
	{
		var permissions = Enum
			   .GetValues<PermissionEnum>()
			   .Select(p => new Permission
			   {
				   Id = (int)p,
				   Name = p.ToString()
			   });

		builder.HasData(permissions);
	}
}
