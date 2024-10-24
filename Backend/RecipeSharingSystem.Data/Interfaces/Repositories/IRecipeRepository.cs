using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IRecipeRepository : IAbstractRepository<Recipe>
{
    Task<Recipe> GetWithDetailsByIdAsync(Guid id);
    Task<ICollection<Recipe>> GetAllWithDetailsAsync();
}
