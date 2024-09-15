using RecipeSharingSystem.Business.DTOs.Instruction;

namespace RecipeSharingSystem.Business.Services.Interfaces
{
	public interface IInstructionService
	{
		Task<InstructionDto> CreateInstructionAsync(CreateInstructionRequestDto model);
		Task<IEnumerable<InstructionDto>> GetAllInstructionsAsync();
		Task<InstructionDto> GetInstructionByIdAsync(Guid id);
		Task<InstructionDto> UpdateInstructionAsync(Guid id, UpdateInstructionRequestDto model);
		Task<InstructionDto> DeleteInstructionAsync(Guid id);
	}
}
