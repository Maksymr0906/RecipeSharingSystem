using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class CommentRepository(RecipeSharingSystemDbContext context)
	: Repository<Comment>(context), ICommentRepository
{
}
