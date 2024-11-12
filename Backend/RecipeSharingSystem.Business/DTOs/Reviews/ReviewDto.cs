namespace RecipeSharingSystem.Business.DTOs.Review;

public record ReviewDto
{
	public required Guid Id { get; set; }
	public required int Rating { get; set; }
	public string? Content { get; set; }
	public required DateTime DateCreated {  get; set; }
	public required Guid UserId { get; set; }
	public required Guid RecipeId { get; set; }
}