using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data.Repositories.Implementation
{
	public class ImageRepository : AbstractRepository<Image>, IImageRepository
	{
		public ImageRepository(RecipeSharingSystemDbContext context)
			: base(context)
		{
		}
	}
}
