using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface ICategoryRepository : IAbstractRepository<Category>
{
    Task<List<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> categoryIds);
}

