using AutoMapper;
using RecipeSharingSystem.Business.DTOs.User;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _repository;
		private readonly IMapper _mapper;

		public UserService(IUserRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<UserDto> CreateUserAsync(CreateUserRequestDto model)
		{
			var user = _mapper.Map<User>(model);
			user = await _repository.CreateAsync(user);
			return _mapper.Map<UserDto>(user);
		}

		public async Task<ICollection<UserDto>> GetAllUsersAsync()
		{
			var users = await _repository.GetAllAsync();
			return _mapper.Map<ICollection<UserDto>>(users);
		}

		public async Task<UserDto> GetUserByIdAsync(Guid id)
		{
			var users = await _repository.GetByIdAsync(id);
			return _mapper.Map<UserDto>(users);
		}

		public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserRequestDto model)
		{
			var user = _mapper.Map<User>(model);
			user.Id = id;
			user = await _repository.UpdateAsync(user);
			return _mapper.Map<UserDto>(user);
		}

		public async Task<UserDto> DeleteUserAsync(Guid id)
		{
			var user = await _repository.DeleteByIdAsync(id);
			return _mapper.Map<UserDto>(user);
		}
	}
}
