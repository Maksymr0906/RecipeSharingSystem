﻿namespace RecipeSharingSystem.Business.DTOs.Comment
{
	public class CreateCommentRequestDto
	{
		public string Content { get; set; }
		public Guid UserId { get; set; }
		public Guid RecipeId { get; set; }
	}
}
