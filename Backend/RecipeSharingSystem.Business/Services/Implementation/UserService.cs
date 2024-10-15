using AutoMapper;
using RecipeSharingSystem.Business.DTOs.User;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper) : IUserService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;

	public async Task<UserDto> CreateUserAsync(CreateUserRequestDto model)
	{
		var user = _mapper.Map<User>(model);
		user = await _unitOfWork.UserRepository.CreateAsync(user);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<UserDto>(user);
	}

	public async Task<ICollection<UserDto>> GetAllUsersAsync()
	{
		var users = await _unitOfWork.UserRepository.GetAllAsync();
		return _mapper.Map<ICollection<UserDto>>(users);
	}

	public async Task<UserDto> GetUserByIdAsync(Guid id)
	{
		var users = await _unitOfWork.UserRepository.GetByIdAsync(id);
		return _mapper.Map<UserDto>(users);
	}

	public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserRequestDto model)
	{
		var user = _mapper.Map<User>(model);
		user.Id = id;
		user = await _unitOfWork.UserRepository.UpdateAsync(user);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<UserDto>(user);
	}

	public async Task<UserDto> DeleteUserAsync(Guid id)
	{
		var user = await _unitOfWork.UserRepository.DeleteByIdAsync(id);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<UserDto>(user);
	}
}
