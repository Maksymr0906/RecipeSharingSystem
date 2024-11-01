using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class ImageRepository(RecipeSharingSystemDbContext context)
	: Repository<Image>(context), IImageRepository
{
}
