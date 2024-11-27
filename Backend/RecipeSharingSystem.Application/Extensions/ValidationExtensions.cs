using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RecipeSharingSystem.Application.Services.Implementation;
using RecipeSharingSystem.Core.Interfaces.Services;
using System.Reflection;

namespace RecipeSharingSystem.Application.Extensions;

public static class ValidationExtensions
{
	public static IServiceCollection AddValidators(this IServiceCollection services)
	{
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddScoped<IValidationService, ValidationService>();

		return services;
	}
}
