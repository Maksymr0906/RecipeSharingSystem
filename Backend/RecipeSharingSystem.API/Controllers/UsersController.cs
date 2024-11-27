using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.User;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _service;

		public UsersController(IUserService service)
		{
			_service = service;
		}

		[HttpGet("{id:Guid}")]
		public async Task<IActionResult> GetUserById([FromRoute] Guid id)
		{
			var response = await _service.GetUserByIdAsync(id);
			if (response == null)
			{
				return NotFound();
			}

			return Ok(response);
		}

		[HttpPut("{id:Guid}")]
		public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequestDto request)
		{
			try
			{
				await _service.UpdateUserAsync(id, request);

				return Ok();
			}
			catch (ValidationException ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
