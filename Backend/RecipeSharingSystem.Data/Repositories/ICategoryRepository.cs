using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Core.Repositories
{
    public interface ICategoryRepository : IAbstractRepository<Category>
    {
        Task<List<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> categoryIds);
    }
}
