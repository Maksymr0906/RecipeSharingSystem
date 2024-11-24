using AutoMapper;
using RecipeSharingSystem.Application.DTOs.Auth;
using RecipeSharingSystem.Application.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Application.Services.Implementation;

public class AuthService(
	IUnitOfWork unitOfWork,
	IMapper mapper,
	IPasswordHasher passwordHasher,
	IJwtProvider jwtProvider) : IAuthService
{
	private readonly IPasswordHasher _passwordHasher = passwordHasher;
	private readonly IJwtProvider _jwtProvider = jwtProvider;
	private readonly IMapper _mapper = mapper;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task Register(RegisterRequestDto model)
	{
		var hashedPassword = _passwordHasher.Generate(model.Password);
		var user = _mapper.Map<User>(model);
		user.PasswordHash = hashedPassword;
		await _unitOfWork.UserRepository.CreateAsync(user);
		await _unitOfWork.SaveAsync();
	}

	public async Task<LoginResponseDto> Login(LoginRequestDto model)
	{
		var user = await _unitOfWork.UserRepository.GetByEmail(model.Email);
		var result = _passwordHasher.Verify(model.Password, user.PasswordHash);
		if (result == false)
		{
			throw new Exception("Wrong password");
		}

		var token = _jwtProvider.Generate(user);

		var response = new LoginResponseDto
		(
			user.Id.ToString(),
			user.Email,
			token,
			user.UserRoles.Select(ur => ur.Role.Name).ToList()
		);

		return response;
	}
}
