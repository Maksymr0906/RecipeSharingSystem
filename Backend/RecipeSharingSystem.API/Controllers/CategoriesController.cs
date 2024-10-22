using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Category;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryService _service;

		public CategoriesController(ICategoryService service) 
		{
			_service = service;
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
		{
			var category = await _service.CreateCategoryAsync(request);
			return Ok(category);
		}

		[Authorize(Policy = "ReadPolicy")]
		[HttpGet]
		public async Task<IActionResult> GetAllCategories()
		{
			var categories = await _service.GetAllCategoriesAsync();
			return Ok(categories);
		}

		[HttpGet("{id:Guid}")]
		public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
		{
			var category = await _service.GetCategoryByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			return Ok(category);
		}

		[HttpPut("{id:Guid}")]
		public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto request)
		{
			var category = await _service.UpdateCategoryAsync(id, request);
			if (category == null)
			{
				return NotFound();
			}

			return Ok(category);
		}

		[HttpDelete("{id:Guid}")]
		public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
		{
			var category = await _service.DeleteCategoryAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			return Ok(category);
		}
	}
}
