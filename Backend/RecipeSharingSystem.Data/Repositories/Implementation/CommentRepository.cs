﻿using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data.Repositories.Implementation
{
	public class CommentRepository : AbstractRepository<Comment>, ICommentRepository
	{
		public CommentRepository(RecipeSharingSystemDbContext context)
			: base(context)
		{
		}
	}
}