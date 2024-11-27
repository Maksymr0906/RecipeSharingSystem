using AutoMapper;
using RecipeSharingSystem.Business.DTOs.User;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
	: IUserService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;
	private readonly IValidationService _validationService = validationService;

	public async Task<ICollection<UserDto>> GetAllUsersAsync()
	{
		var users = await _unitOfWork.UserRepository.GetAllAsync();

		return _mapper.Map<ICollection<UserDto>>(users);
	}

	public async Task<UserDto> GetUserByIdAsync(Guid id)
	{
		var users = await _unitOfWork.UserRepository.GetByIdWithDetails(id);

		return _mapper.Map<UserDto>(users);
	}

	public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserRequestDto model)
	{
		await _validationService.ValidateAsync(model);

		var existingUser = await _unitOfWork.UserRepository.GetByIdWithDetails(id);

		_mapper.Map(model, existingUser);

		await _unitOfWork.UserRepository.UpdateAsync(existingUser);

		await _unitOfWork.SaveAsync();

		return _mapper.Map<UserDto>(existingUser);
	}

	public async Task<UserDto> DeleteUserAsync(Guid id)
	{
		var user = await _unitOfWork.UserRepository.DeleteByIdAsync(id);

		await _unitOfWork.SaveAsync();

		return _mapper.Map<UserDto>(user);
	}
}
