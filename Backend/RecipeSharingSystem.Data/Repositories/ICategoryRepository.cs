using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Repositories;

public interface ICategoryRepository : IAbstractRepository<Category>
{
	Task<List<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> categoryIds);
}

