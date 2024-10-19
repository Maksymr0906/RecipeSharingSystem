using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Application.DTOs.Auth;
using RecipeSharingSystem.Application.Interfaces;
using RecipeSharingSystem.Application.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService)
		: ControllerBase
	{
		private readonly IAuthService _authService = authService;

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
		{
			await _authService.Register(request);
			return Ok();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
		{
			var response = await _authService.Login(request);
			return Ok(response);
		}
	}
}
