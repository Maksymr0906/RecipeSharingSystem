using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Instruction;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InstructionsController : ControllerBase
	{
		private readonly IInstructionService _service;

		public InstructionsController(IInstructionService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<IActionResult> CreateInstruction([FromBody] CreateInstructionRequestDto request)
		{
			var instruction = await _service.CreateInstructionAsync(request);
			return Ok(instruction);
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

		[HttpPut("{id:Guid}")]
		public async Task<IActionResult> UpdateInstruction([FromRoute] Guid id, [FromBody] UpdateInstructionRequestDto request)
		{
			var instruction = await _service.UpdateInstructionAsync(id, request);
			if (instruction == null)
			{
				return NotFound();
			}

			return Ok(instruction);
		}

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
}
