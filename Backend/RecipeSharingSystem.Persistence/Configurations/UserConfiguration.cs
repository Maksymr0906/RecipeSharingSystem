using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class UserConfiguration(AuthorizationOptions authorization)
	: IEntityTypeConfiguration<User>
{
	private readonly AuthorizationOptions _authorization = authorization;

	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasData(_authorization.Users);
	}
}
