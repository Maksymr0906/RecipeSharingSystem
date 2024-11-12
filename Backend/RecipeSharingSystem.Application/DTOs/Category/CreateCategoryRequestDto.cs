namespace RecipeSharingSystem.Business.DTOs.Category;

public record CreateCategoryRequestDto(
	string Name,
	string Slug, 
	string FeaturedImageUrl
);