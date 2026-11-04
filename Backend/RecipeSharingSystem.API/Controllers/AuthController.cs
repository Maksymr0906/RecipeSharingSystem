using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RecipeSharingSystem.Application.DTOs.Auth;
using RecipeSharingSystem.Application.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers;

// Limits password guessing and mass registration (see the "auth" policy in Program.cs).
[EnableRateLimiting("auth")]
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService)
	: ControllerBase
{
	private readonly IAuthService _authService = authService;

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
	{
		try
		{
			await _authService.Register(request);

			return Ok();
		}
		catch (ValidationException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
	{
		try
		{
			var response = await _authService.Login(request);

			return Ok(response);
		}
		catch(ValidationException ex)
		{
			return BadRequest(ex.Message);
		}
	}
}
