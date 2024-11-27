namespace RecipeSharingSystem.Core.Interfaces.Services;

public interface IValidationService
{
	Task ValidateAsync<T>(T model);
}