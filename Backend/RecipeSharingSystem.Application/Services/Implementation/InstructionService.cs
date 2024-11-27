using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Instruction;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class InstructionService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
	: IInstructionService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;
	private readonly IValidationService _validationService = validationService;

	public async Task<InstructionDto> CreateInstructionAsync(CreateInstructionRequestDto model)
	{
		await _validationService.ValidateAsync(model);

		var instruction = _mapper.Map<Instruction>(model);

		instruction = await _unitOfWork.InstructionRepository.CreateAsync(instruction);

		await _unitOfWork.SaveAsync();

		return _mapper.Map<InstructionDto>(instruction);
	}

	public async Task<ICollection<InstructionDto>> GetAllInstructionsAsync()
	{
		var instructions = await _unitOfWork.InstructionRepository.GetAllAsync();

		return _mapper.Map<ICollection<InstructionDto>>(instructions);
	}

	public async Task<InstructionDto> GetInstructionByIdAsync(Guid id)
	{
		var instruction = await _unitOfWork.InstructionRepository.GetByIdAsync(id);

		return _mapper.Map<InstructionDto>(instruction);
	}

	public async Task<InstructionDto> UpdateInstructionAsync(Guid id, UpdateInstructionRequestDto model)
	{
		await _validationService.ValidateAsync(model);

		var instruction = _mapper.Map<Instruction>(model);

		instruction.Id = id;
		
		instruction = await _unitOfWork.InstructionRepository.UpdateAsync(instruction);
		
		await _unitOfWork.SaveAsync();
		
		return _mapper.Map<InstructionDto>(instruction);
	}

	public async Task<InstructionDto> DeleteInstructionAsync(Guid id)
	{
		var instruction = await _unitOfWork.IngredientRepository.DeleteByIdAsync(id);
		
		await _unitOfWork.SaveAsync();
		
		return _mapper.Map<InstructionDto>(instruction);
	}
}
