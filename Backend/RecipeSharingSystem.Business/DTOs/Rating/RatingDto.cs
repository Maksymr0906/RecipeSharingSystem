namespace RecipeSharingSystem.Business.DTOs.Rating
{
	public class RatingDto
	{
		public Guid Id { get; set; }
		public int Value { get; set; }
		public Guid UserId { get; set; }
		public Guid RecipeId { get; set; }
	}
}
