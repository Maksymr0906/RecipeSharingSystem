using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RecipeSharingSystem.Application.Validation;
using System.Reflection;

namespace RecipeSharingSystem.Application.Extensions;

public static class ValidationExtensions
{
	public static IServiceCollection AddValidators(this IServiceCollection services)
	{
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddScoped(typeof(IValidationBehavior<,>), typeof(ValidationBehavior<,>));

		return services;
	}
}
