namespace RecipeSharingSystem.Business.DTOs.Comment;

public record CommentDto(
	Guid Id,
	string Content,
	DateTime DateCreated,
	Guid UserId,
	Guid RecipeId
);