using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> categoryIds);
    Task<Category> GetCategoryBySlugAsync(string slug);
}

