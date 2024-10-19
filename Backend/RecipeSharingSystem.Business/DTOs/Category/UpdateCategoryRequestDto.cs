namespace RecipeSharingSystem.Business.DTOs.Category;

public record UpdateCategoryRequestDto(
	string Name,
	string Slug,
	string FeaturedImageUrl
);