namespace RecipeSharingSystem.Business.DTOs.Comment;

public record CreateCommentRequestDto(
	string Content,
	Guid UserId,
	Guid RecipeId
);