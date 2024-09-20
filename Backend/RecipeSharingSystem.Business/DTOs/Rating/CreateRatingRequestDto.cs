namespace RecipeSharingSystem.Business.DTOs.Rating
{
	public class CreateRatingRequestDto
	{
		public int Value { get; set; }
		public Guid UserId { get; set; }
		public Guid RecipeId { get; set; }
	}
}
