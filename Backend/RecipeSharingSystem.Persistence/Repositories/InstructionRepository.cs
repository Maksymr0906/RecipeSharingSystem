using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class InstructionRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Instruction>(context), IInstructionRepository
{
}
