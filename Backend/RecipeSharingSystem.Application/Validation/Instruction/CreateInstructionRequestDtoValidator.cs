using FluentValidation;
using RecipeSharingSystem.Business.DTOs.Instruction;

namespace RecipeSharingSystem.Application.Validation.Instruction;

public class CreateInstructionRequestDtoValidator : BaseValidator<CreateInstructionRequestDto>
{
	public CreateInstructionRequestDtoValidator()
	{
		RuleFor(x => x.Content)
			.NotEmpty()
			.MaximumLength(10000)
			.WithMessage("Instruction content is required and must be less than 10000 characters");
	}
}
