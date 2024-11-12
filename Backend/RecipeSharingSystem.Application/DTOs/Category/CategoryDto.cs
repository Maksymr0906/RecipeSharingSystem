namespace RecipeSharingSystem.Business.DTOs.Category;

public record CategoryDto(
	Guid Id,
	string Name,
	string Slug,
	string FeaturedImageUrl
);