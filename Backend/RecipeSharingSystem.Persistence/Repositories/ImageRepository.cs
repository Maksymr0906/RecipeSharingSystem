using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories
{
    public class ImageRepository : AbstractRepository<Image>, IImageRepository
    {
        public ImageRepository(RecipeSharingSystemDbContext context)
            : base(context)
        {
        }
    }
}
