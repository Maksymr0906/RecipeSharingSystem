﻿using AutoMapper;
using FluentValidation;
using RecipeSharingSystem.Business.DTOs.Category;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
	: ICategoryService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;
	private readonly IValidationService _validationService = validationService;

	public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto model)
	{
		await _validationService.ValidateAsync(model);

		var category = _mapper.Map<Category>(model);
		
		category = await _unitOfWork.CategoryRepository.CreateAsync(category);
		
		await _unitOfWork.SaveAsync();
		
		return _mapper.Map<CategoryDto>(category);
	}

	public async Task<ICollection<CategoryDto>> GetAllCategoriesAsync()
	{
		var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
		
		return _mapper.Map<ICollection<CategoryDto>>(categories);
	}

	public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
	{
		var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
		
		return _mapper.Map<CategoryDto>(category);
	}

	public async Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto model)
	{
		await _validationService.ValidateAsync(model);

		var category = _mapper.Map<Category>(model);
		
		category.Id = id;
		
		category = await _unitOfWork.CategoryRepository.UpdateAsync(category);
		
		await _unitOfWork.SaveAsync();
		
		return _mapper.Map<CategoryDto>(category);
	}

	public async Task<CategoryDto> DeleteCategoryAsync(Guid id)
	{
		var category = await _unitOfWork.CategoryRepository.DeleteByIdAsync(id);
		
		await _unitOfWork.SaveAsync();
		
		return _mapper.Map<CategoryDto>(category);
	}

	public async Task<ICollection<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> categoryIds)
	{
		return await _unitOfWork.CategoryRepository.GetCategoriesByIdsAsync(categoryIds);
	}

	public async Task<CategoryDto> GetCategoryBySlugAsync(string slug)
	{
		var category = await _unitOfWork.CategoryRepository.GetCategoryBySlugAsync(slug);

		return _mapper.Map<CategoryDto>(category);
	}
}
