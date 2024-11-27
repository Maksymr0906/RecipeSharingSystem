﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Application.Services.Implementation;

public class ValidationService : IValidationService
{
	private readonly IServiceProvider _serviceProvider;

	public ValidationService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public async Task ValidateAsync<T>(T model)
	{
		var validator = _serviceProvider.GetService<IValidator<T>>();

		if (validator == null)
		{
			throw new InvalidOperationException($"No validator found for type {typeof(T).Name}");
		}

		var validationResult = await validator.ValidateAsync(model);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
	}
}