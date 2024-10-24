using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class RoleConfiguration
	: IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		var roles = Enum
				.GetValues<RoleType>()
				.Select(r => new Role
				{
					Id = (int)r,
					Name = r.ToString()
				});

		builder.HasData(roles);
	}
}
