using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class ImageRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Image>(context), IImageRepository
{
}
