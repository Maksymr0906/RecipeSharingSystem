using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.SeedData;

namespace RecipeSharingSystem.Persistence.Configurations;

public partial class InstructionConfiguration(SeedDataOptions seedDataOptions)
	: IEntityTypeConfiguration<Instruction>
{
	private readonly SeedDataOptions _seedDataOptions = seedDataOptions;

	public void Configure(EntityTypeBuilder<Instruction> builder)
	{
		builder.HasData(_seedDataOptions.Instructions);
	}
}
