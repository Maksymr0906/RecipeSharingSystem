using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories
{
    public class IngredientRepository : AbstractRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(RecipeSharingSystemDbContext context)
            : base(context)
        {
        }

        public async Task<Ingredient> GetByNameAsync(string name)
        {
            return await Entities.FirstOrDefaultAsync(ingredient => ingredient.Name == name);
        }

        public async Task<IEnumerable<Ingredient>> GetByNamesAsync(IEnumerable<string> names)
        {
            return await Entities
                .Where(ingredient => names.Contains(ingredient.Name))
                .ToListAsync();
        }
    }
}
