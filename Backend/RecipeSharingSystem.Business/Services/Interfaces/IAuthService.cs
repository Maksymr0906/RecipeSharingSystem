using RecipeSharingSystem.Application.DTOs.Auth;

namespace RecipeSharingSystem.Application.Services.Interfaces;

public interface IAuthService
{
	Task Register(RegisterRequestDto model);
	Task<LoginResponseDto> Login(LoginRequestDto model);
}
