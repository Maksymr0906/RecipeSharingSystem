using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Category;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _repository;
		private readonly IMapper _mapper;

		public CategoryService(ICategoryRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto model)
		{
			var category = _mapper.Map<Category>(model);
			category = await _repository.CreateAsync(category);
			return _mapper.Map<CategoryDto>(category);
		}

		public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
		{
			var categories = await _repository.GetAllAsync();
			return _mapper.Map<IEnumerable<CategoryDto>>(categories);
		}

		public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
		{
			var category = await _repository.GetByIdAsync(id);
			return _mapper.Map<CategoryDto>(category);
		}

		public async Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto model)
		{
			var category = _mapper.Map<Category>(model);
			category.Id = id;
			category = await _repository.UpdateAsync(category);
			return _mapper.Map<CategoryDto>(category);
		}

		public async Task<CategoryDto> DeleteCategoryAsync(Guid id)
		{
			var category = await _repository.DeleteByIdAsync(id);
			return _mapper.Map<CategoryDto>(category);
		}
	}
}
