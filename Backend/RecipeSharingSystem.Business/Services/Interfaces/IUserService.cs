using RecipeSharingSystem.Business.DTOs.User;
using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Business.Services.Interfaces;

public interface IUserService
{
	Task<UserDto> CreateUserAsync(CreateUserRequestDto model);
	Task<ICollection<UserDto>> GetAllUsersAsync();
	Task<UserDto> GetUserByIdAsync(Guid id);
	Task<UserDto> UpdateUserAsync(Guid id, UpdateUserRequestDto model);
	Task<UserDto> DeleteUserAsync(Guid id);
	Task<HashSet<PermissionEnum>> GetUserPermissions(Guid userId);
}
