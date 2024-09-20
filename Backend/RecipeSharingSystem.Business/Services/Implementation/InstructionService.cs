using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Ingredient;
using RecipeSharingSystem.Business.DTOs.Instruction;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class InstructionService : IInstructionService
	{
		private readonly IInstructionRepository _repository;
		private readonly IMapper _mapper;

		public InstructionService(IInstructionRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<InstructionDto> CreateInstructionAsync(CreateInstructionRequestDto model)
		{
			var instruction = _mapper.Map<Instruction>(model);
			instruction = await _repository.CreateAsync(instruction);
			return _mapper.Map<InstructionDto>(instruction);
		}

		public async Task<ICollection<InstructionDto>> GetAllInstructionsAsync()
		{
			var instructions = await _repository.GetAllAsync();
			return _mapper.Map<ICollection<InstructionDto>>(instructions);
		}

		public async Task<InstructionDto> GetInstructionByIdAsync(Guid id)
		{
			var instruction = await _repository.GetByIdAsync(id);
			return _mapper.Map<InstructionDto>(instruction);
		}

		public async Task<InstructionDto> UpdateInstructionAsync(Guid id, UpdateInstructionRequestDto model)
		{
			var instruction = _mapper.Map<Instruction>(model);
			instruction.Id = id;
			instruction = await _repository.UpdateAsync(instruction);
			return _mapper.Map<InstructionDto>(instruction);
		}

		public async Task<InstructionDto> DeleteInstructionAsync(Guid id)
		{
			var instruction = await _repository.DeleteByIdAsync(id);
			return _mapper.Map<InstructionDto>(instruction);
		}
	}
}
