using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Category;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService service)
	: ControllerBase
{
	private readonly ICategoryService _service = service;

	[Authorize(Policy = "CreatePolicy")]
	[HttpPost]
	public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
	{
		var category = await _service.CreateCategoryAsync(request);
		return Ok(category);
	}

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

	[Authorize(Policy = "UpdatePolicy")]
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

	[Authorize(Policy = "DeletePolicy")]
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

	[HttpGet("slug/{slug}")]
	public async Task<IActionResult> GetCategoryBySlug([FromRoute] string slug)
	{
		var category = await _service.GetCategoryBySlugAsync(slug);
		if (category == null)
		{
			return NotFound();
		}

		return Ok(category);
	}
}
