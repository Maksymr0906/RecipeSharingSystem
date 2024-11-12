using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Image;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController(IImageService service, IWebHostEnvironment webHostEnvironment)
	: ControllerBase
{
	private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
	private readonly IImageService _service = service;

	[Authorize(Policy = "CreatePolicy")]
	[HttpPost]
	public async Task<IActionResult> CreateImage([FromForm] IFormFile file,
		[FromForm] string fileName, [FromForm] string title)
	{
		try
		{
			var fileExtension = Path.GetExtension(file.FileName);
			var httpRequest = HttpContext.Request;
			var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{fileName}{fileExtension}";
			var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{fileName}{fileExtension}");

			var imageUploadModel = new ImageUploadModel(default, default, default, default, default, default)
			{
				FileName = fileName,
				FileExtension = fileExtension,
				Title = title,
				LocalPath = localPath,
				UrlPath = urlPath,
				FileContent = await ConvertFileToByteArrayAsync(file),
			};

			var image = await _service.CreateImageAsync(imageUploadModel);
			return Ok(image);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpGet]
	public async Task<IActionResult> GetAllImages()
	{
		var images = await _service.GetAllImagesAsync();
		return Ok(images);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetImageById([FromRoute] Guid id)
	{
		var image = await _service.GetImageByIdAsync(id);
		if (image == null)
		{
			return NotFound();
		}

		return Ok(image);
	}

	private async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
	{
		using var memoryStream = new MemoryStream();
		await file.CopyToAsync(memoryStream);
		return memoryStream.ToArray();
	}
}
