using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class CommentRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<Comment>(context), ICommentRepository
{
}
