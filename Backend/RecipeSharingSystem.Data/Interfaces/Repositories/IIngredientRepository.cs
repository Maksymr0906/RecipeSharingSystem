using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IIngredientRepository : IRepository<Ingredient>
{
    Task<IEnumerable<Ingredient>> GetByNamesAsync(IEnumerable<string> names);
    Task<Ingredient> GetByNameAsync(string name);
}
