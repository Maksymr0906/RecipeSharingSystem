using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class InstructionRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Instruction>(context), IInstructionRepository
{
}
