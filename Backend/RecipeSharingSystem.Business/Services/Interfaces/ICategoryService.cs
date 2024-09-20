using RecipeSharingSystem.Business.DTOs.Category;

namespace RecipeSharingSystem.Business.Services.Interfaces
{
	public interface ICategoryService
	{
		Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto model);
		Task<ICollection<CategoryDto>> GetAllCategoriesAsync();
		Task<CategoryDto> GetCategoryByIdAsync(Guid id);
		Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto model);
		Task<CategoryDto> DeleteCategoryAsync(Guid id);
	}
}
