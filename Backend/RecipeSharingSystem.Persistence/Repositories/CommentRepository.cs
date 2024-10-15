using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Persistence.Repositories
{
    public class CommentRepository : AbstractRepository<Comment>, ICommentRepository
    {
        public CommentRepository(RecipeSharingSystemDbContext context)
            : base(context)
        {
        }
    }
}
