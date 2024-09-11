using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data.Repositories.Implementation
{
	public class CategoryRepository : AbstractRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(RecipeSharingSystemDbContext context) 
			: base(context)
		{
		}
	}
}
