using FluentValidation;
using RecipeSharingSystem.Business.DTOs.Ingredient;

namespace RecipeSharingSystem.Application.Validation.Ingredient;

public class IngredientDtoValidator : BaseValidator<IngredientDto>
{
	public IngredientDtoValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.WithMessage("Ingredient ID is required");

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100)
			.WithMessage("Ingredient name is required and must be less than 100 characters");

		RuleFor(x => x.Slug)
			.NotEmpty()
			.MaximumLength(150)
			.Matches("^[a-z0-9-]+$")
			.WithMessage("Slug must be lowercase, alphanumeric, and can contain hyphens");
	}
}