using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class UserConfiguration
	: IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		throw new NotImplementedException();
	}
}
