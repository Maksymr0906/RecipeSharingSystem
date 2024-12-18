﻿using RecipeSharingSystem.Business.DTOs.User;

namespace RecipeSharingSystem.Business.Services.Interfaces;

public interface IUserService
{
	Task<ICollection<UserDto>> GetAllUsersAsync();
	Task<UserDto> GetUserByIdAsync(Guid id);
	Task<UserDto> UpdateUserAsync(Guid id, UpdateUserRequestDto model);
	Task<UserDto> DeleteUserAsync(Guid id);
}
