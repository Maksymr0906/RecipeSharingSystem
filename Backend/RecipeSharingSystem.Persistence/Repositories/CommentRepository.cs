using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories;

public class CommentRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Comment>(context), ICommentRepository
{
}
