using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories
{
    public class InstructionRepository : AbstractRepository<Instruction>, IInstructionRepository
    {
        public InstructionRepository(RecipeSharingSystemDbContext context)
            : base(context)
        {
        }
    }
}
