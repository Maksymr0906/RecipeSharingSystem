using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Category;
using RecipeSharingSystem.Business.DTOs.Image;
using RecipeSharingSystem.Business.DTOs.Ingredient;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Business
{
	public class AutomapperProfile : Profile
	{
		public AutomapperProfile() 
		{
			// Category
			CreateMap<CreateCategoryRequestDto, Category>()
				.ForMember(c => c.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(c => c.Image, opt => opt.Ignore());

			CreateMap<UpdateCategoryRequestDto, Category>()
				.ForMember(c => c.Image, opt => opt.Ignore());

			CreateMap<Category, CategoryDto>();

			// Image
			CreateMap<ImageUploadModel, Image>()
				.ForMember(i => i.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(i => i.DateCreated, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(i => i.Url, opt => opt.MapFrom(im => im.UrlPath))
				.ForMember(i => i.Categories, opt => opt.Ignore())
				.ForMember(i => i.Ingredients, opt => opt.Ignore())
				.ForMember(i => i.Recipes, opt => opt.Ignore());

			CreateMap<Image, ImageDto>();

			// Ingredient
			CreateMap<CreateIngredientRequestDto, Ingredient>()
				.ForMember(i => i.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(i => i.Image, opt => opt.Ignore())
				.ForMember(i => i.RecipeIngredients, opt => opt.Ignore());

			CreateMap<UpdateIngredientRequestDto, Ingredient>()
				.ForMember(i => i.Image, opt => opt.Ignore())
				.ForMember(i => i.RecipeIngredients, opt => opt.Ignore());

			CreateMap<Ingredient, IngredientDto>();
		}
	}
}
