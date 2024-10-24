using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Persistence.Repositories;

namespace RecipeSharingSystem.Persistence;

public static class PersistenceExtenstions
{
	public static IServiceCollection AddPersistence(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<RecipeSharingSystemDbContext>(options =>
		{
			options.UseMySql(configuration.GetConnectionString("RecipeSharingSystemConnectionString"), new MySqlServerVersion(new Version(8, 3, 0)));
		});

		services.AddScoped<ICategoryRepository, CategoryRepository>();
		services.AddScoped<ICommentRepository, CommentRepository>();
		services.AddScoped<IImageRepository, ImageRepository>();
		services.AddScoped<IIngredientRepository, IngredientRepository>();
		services.AddScoped<IInstructionRepository, InstructionRepository>();
		services.AddScoped<IRatingRepository, RatingRepository>();
		services.AddScoped<IRecipeRepository, RecipeRepository>();
		services.AddScoped<IUserRepository, UserRepository>();

		services.AddScoped<IUnitOfWork, UnitOfWork>();

		return services;
	}
}
