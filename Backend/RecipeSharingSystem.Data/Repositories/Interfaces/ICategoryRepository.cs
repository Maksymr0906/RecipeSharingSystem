using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Data.Repositories.Interfaces
{
	public interface ICategoryRepository : IAbstractRepository<Category>
	{
		Task<List<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> categoryIds);
	}
}
