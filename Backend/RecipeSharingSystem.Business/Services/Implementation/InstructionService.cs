using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Instruction;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class InstructionService : IInstructionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public InstructionService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<InstructionDto> CreateInstructionAsync(CreateInstructionRequestDto model)
		{
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
}
