using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class InstructionRepository(RecipeSharingSystemDbContext context)
	: Repository<Instruction>(context), IInstructionRepository
{
}
