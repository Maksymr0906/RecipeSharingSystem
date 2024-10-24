using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class InstructionRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Instruction>(context), IInstructionRepository
{
}
