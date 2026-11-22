using AutoMapper;
using FluentValidation;
using RecipeSharingSystem.Application.DTOs.Auth;
using RecipeSharingSystem.Application.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;
using RecipeSharingSystem.Core.Exceptions;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Application.Services.Implementation;

public class AuthService(
	IUnitOfWork unitOfWork,
	IMapper mapper,
	IPasswordHasher passwordHasher,
	IJwtProvider jwtProvider,
	IValidationService validationService,
	ISecurityEventLogger securityEventLogger) : IAuthService
{
	// One message for every authentication failure: it must not reveal whether the account exists.
	private const string InvalidCredentialsMessage = "Invalid email or password.";

	private readonly IPasswordHasher _passwordHasher = passwordHasher;
	private readonly IJwtProvider _jwtProvider = jwtProvider;
	private readonly IValidationService _validationService = validationService;
	private readonly IMapper _mapper = mapper;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ISecurityEventLogger _securityEventLogger = securityEventLogger;

	public async Task Register(RegisterRequestDto model)
	{
		await _validationService.ValidateAsync(model);

		// Identifiers (email, username) must be unique. The database also enforces this with unique indexes.
		if (await _unitOfWork.UserRepository.ExistsByEmailOrUserNameAsync(model.Email, model.UserName))
		{
			throw new ValidationException("A user with this email or username already exists.");
		}

		var user = _mapper.Map<User>(model);

		user.PasswordHash = _passwordHasher.Generate(model.Password);

		// Least privilege: every new account starts with the standard User role. Admin can only be granted by an admin.
		user.UserRoles.Add(new UserRole
		{
			UserId = user.Id,
			RoleId = (int)RoleType.User
		});

		await _unitOfWork.UserRepository.CreateAsync(user);

		await _unitOfWork.SaveAsync();
	}

	public async Task<LoginResponseDto> Login(LoginRequestDto model)
	{
		await _validationService.ValidateAsync(model);

		var user = await _unitOfWork.UserRepository.FindByEmailAsync(model.Email);

		if (user is null)
		{
			// Spend comparable time as for an existing account, then fail with the same message.
			_passwordHasher.SimulateVerification(model.Password);

			_securityEventLogger.AuthenticationFailed(model.Email, "no account with this email");

			throw new AuthenticationFailedException(InvalidCredentialsMessage);
		}

		if (!_passwordHasher.Verify(model.Password, user.PasswordHash))
		{
			_securityEventLogger.AuthenticationFailed(model.Email, "password did not match");

			throw new AuthenticationFailedException(InvalidCredentialsMessage);
		}

		_securityEventLogger.AuthenticationSucceeded(user.Id);

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
