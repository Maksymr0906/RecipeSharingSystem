using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IRecipeRepository : IRepository<Recipe>
{
    Task<Recipe> GetWithDetailsByIdAsync(Guid id);
    Task<ICollection<Recipe>> GetAllWithDetailsAsync();
}
