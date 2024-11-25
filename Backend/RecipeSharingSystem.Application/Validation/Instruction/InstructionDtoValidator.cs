using FluentValidation;
using RecipeSharingSystem.Business.DTOs.Instruction;

namespace RecipeSharingSystem.Application.Validation.Instruction;

public class InstructionDtoValidator : BaseValidator<InstructionDto>
{
	public InstructionDtoValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.WithMessage("Instruction ID is required");

		RuleFor(x => x.Content)
			.NotEmpty()
			.MaximumLength(10000)
			.WithMessage("Instruction content is required and must be less than 10000 characters");
	}
}