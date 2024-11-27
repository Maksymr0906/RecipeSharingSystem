using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Instruction;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstructionsController(IInstructionService service)
	: ControllerBase
{
	private readonly IInstructionService _service = service;

	[Authorize(Policy = "CreateInstructionPolicy")]
	[HttpPost]
	public async Task<IActionResult> CreateInstruction([FromBody] CreateInstructionRequestDto request)
	{
		try
		{
			var instruction = await _service.CreateInstructionAsync(request);
			
			return Ok(instruction);
		}
		catch (ValidationException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpGet]
	public async Task<IActionResult> GetAllInstructions()
	{
		var instructions = await _service.GetAllInstructionsAsync();
		return Ok(instructions);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetInstructionById([FromRoute] Guid id) 
	{
		var instruction = await _service.GetInstructionByIdAsync(id);
		if (instruction == null)
		{
			return NotFound();
		}

		return Ok(instruction);
	}

	[Authorize(Policy = "UpdateInstructionPolicy")]
	[HttpPut("{id:Guid}")]
	public async Task<IActionResult> UpdateInstruction([FromRoute] Guid id, [FromBody] UpdateInstructionRequestDto request)
	{
		try
		{
			var instruction = await _service.UpdateInstructionAsync(id, request);
			if (instruction == null)
			{
				return NotFound();
			}

			return Ok(instruction);
		}
		catch (ValidationException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[Authorize(Policy = "DeleteInstructionPolicy")]
	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> DeleteInstruction([FromRoute] Guid id)
	{
		var instruction = await _service.DeleteInstructionAsync(id);
		if (instruction == null)
		{
			return NotFound();
		}

		return Ok(instruction);
	}
}
