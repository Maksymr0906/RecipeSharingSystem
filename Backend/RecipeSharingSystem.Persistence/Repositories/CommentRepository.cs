using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class CommentRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Comment>(context), ICommentRepository
{
}
