using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Instruction;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class InstructionService(
	IUnitOfWork unitOfWork,
	IMapper mapper,
	IValidationService validationService,
	IResourceAccessService resourceAccessService)
	: IInstructionService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;
	private readonly IValidationService _validationService = validationService;
	private readonly IResourceAccessService _resourceAccessService = resourceAccessService;

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
		await EnsureCanModifyAsync(id, PermissionType.Update);

		await _validationService.ValidateAsync(model);

		var instruction = _mapper.Map<Instruction>(model);

		instruction.Id = id;
		
		instruction = await _unitOfWork.InstructionRepository.UpdateAsync(instruction);
		
		await _unitOfWork.SaveAsync();
		
		return _mapper.Map<InstructionDto>(instruction);
	}

	public async Task<InstructionDto> DeleteInstructionAsync(Guid id)
	{
		await EnsureCanModifyAsync(id, PermissionType.Delete);

		var instruction = await _unitOfWork.InstructionRepository.DeleteByIdAsync(id);
		
		await _unitOfWork.SaveAsync();
		
		return _mapper.Map<InstructionDto>(instruction);
	}

	// An instruction that already belongs to a recipe may be changed only by that recipe's author or an administrator.
	// A not yet attached instruction (created just before its recipe) is not owned by anyone and stays editable.
	private async Task EnsureCanModifyAsync(Guid instructionId, PermissionType overridePermission)
	{
		var authorId = await _unitOfWork.RecipeRepository.GetAuthorIdByInstructionIdAsync(instructionId);

		if (authorId is not null)
		{
			await _resourceAccessService.EnsureOwnerOrPermissionAsync(authorId.Value, overridePermission);
		}
	}
}
