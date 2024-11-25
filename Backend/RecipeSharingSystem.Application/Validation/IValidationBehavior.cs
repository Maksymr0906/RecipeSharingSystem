﻿namespace RecipeSharingSystem.Application.Validation;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

public interface IValidationBehavior<TRequest, TResponse>
{
	Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next);
}