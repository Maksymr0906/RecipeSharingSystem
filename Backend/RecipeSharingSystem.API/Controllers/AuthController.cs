using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Application.DTOs.Auth;
using RecipeSharingSystem.Application.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers;

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
