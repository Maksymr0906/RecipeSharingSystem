using Microsoft.Extensions.DependencyInjection;
using RecipeSharingSystem.Application.Services.Implementation;
using RecipeSharingSystem.Application.Services.Interfaces;
using RecipeSharingSystem.Business;
using RecipeSharingSystem.Business.Services.Implementation;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.Application;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddAutoMapper(typeof(AutomapperProfile));

		services.AddScoped<ICategoryService, CategoryService>();
		services.AddScoped<ICommentService, CommentService>();
		services.AddScoped<IImageService, ImageService>();
		services.AddScoped<IIngredientService, IngredientService>();
		services.AddScoped<IInstructionService, InstructionService>();
		services.AddScoped<IRatingService, RatingService>();
		services.AddScoped<IRecipeService, RecipeService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IAuthService, AuthService>();

		return services;
	}
}
