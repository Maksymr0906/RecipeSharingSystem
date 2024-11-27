using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IIngredientRepository : IRepository<Ingredient>
{
    Task<Ingredient> GetByNameAsync(string name);
    Task<Ingredient> GetBySlugAsync(string slug);
}
