﻿namespace RecipeSharingSystem.Business.DTOs.Recipe
{
	public class UpdateRecipeRequestDto
	{
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public string FeaturedImageUrl { get; set; }
		public Guid InstructionId { get; set; }
		public IEnumerable<Guid> CategoryIds { get; set; }
		public ICollection<IngredientQuantityDto> Ingredients { get; set; }
	}
}
