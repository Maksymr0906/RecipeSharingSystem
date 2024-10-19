using Microsoft.Extensions.DependencyInjection;
using RecipeSharingSystem.Application.Interfaces;

namespace RecipeSharingSystem.Infrastructure;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddScoped<IPasswordHasher, PasswordHasher>();
		services.AddScoped<IJwtProvider, JwtProvider>();

		return services;
	}
}
