using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class ImageRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Image>(context), IImageRepository
{
}
