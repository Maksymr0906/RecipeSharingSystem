﻿using AutoMapper;
using RecipeSharingSystem.Application.DTOs.Auth;
using RecipeSharingSystem.Business.DTOs.Category;
using RecipeSharingSystem.Business.DTOs.Review;
using RecipeSharingSystem.Business.DTOs.Image;
using RecipeSharingSystem.Business.DTOs.Ingredient;
using RecipeSharingSystem.Business.DTOs.Instruction;
using RecipeSharingSystem.Business.DTOs.Recipe;
using RecipeSharingSystem.Business.DTOs.User;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Business
{
	public class AutomapperProfile : Profile
	{
		public AutomapperProfile() 
		{
			// Category
			CreateMap<CreateCategoryRequestDto, Category>()
				.ForMember(c => c.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));

			CreateMap<UpdateCategoryRequestDto, Category>();

			CreateMap<Category, CategoryDto>();

			// Image
			CreateMap<ImageUploadModel, Image>()
				.ForMember(i => i.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(i => i.DateCreated, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(i => i.Url, opt => opt.MapFrom(im => im.UrlPath));

			CreateMap<Image, ImageDto>();

			// Ingredient
			CreateMap<CreateIngredientRequestDto, Ingredient>()
				.ForMember(i => i.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(i => i.RecipeIngredients, opt => opt.Ignore());

			CreateMap<UpdateIngredientRequestDto, Ingredient>()
				.ForMember(i => i.RecipeIngredients, opt => opt.Ignore());

			CreateMap<Ingredient, IngredientDto>();

			// Instruction
			CreateMap<CreateInstructionRequestDto, Instruction>()
				.ForMember(i => i.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(i => i.Recipe, opt => opt.Ignore());

			CreateMap<UpdateInstructionRequestDto, Instruction>()
				.ForMember(i => i.Recipe, opt => opt.Ignore());

			CreateMap<Instruction, InstructionDto>();

			// User
			CreateMap<UpdateUserRequestDto, User>()
				.ForMember(dest => dest.UserName, opt => opt.Condition((src, dest, srcMember) => src.UserName != null))
				.ForMember(dest => dest.Email, opt => opt.Condition((src, dest, srcMember) => src.Email != null))
				.ForMember(dest => dest.FirstName, opt => opt.Condition((src, dest, srcMember) => src.FirstName != null))
				.ForMember(dest => dest.LastName, opt => opt.Condition((src, dest, srcMember) => src.LastName != null))
				.ForMember(dest => dest.DateOfBirth, opt => opt.Condition((src, dest, srcMember) => src.DateOfBirth != null))
				.ForMember(dest => dest.PostalCode, opt => opt.Condition((src, dest, srcMember) => src.PostalCode != null));

			CreateMap<User, UserDto>()
				.ForMember(dest => dest.AuthoredRecipeIds,
					opt => opt.MapFrom(src => src.AuthoredRecipes.Select(r => r.Id)))
				.ForMember(dest => dest.FavoriteRecipeIds,
					opt => opt.MapFrom(src => src.FavoriteRecipes.Select(fr => fr.RecipeId)))
				.ForMember(dest => dest.ReviewIds,
					opt => opt.MapFrom(src => src.Reviews.Select(r => r.Id)));

			CreateMap<RegisterRequestDto, User>()
				.ForMember(u => u.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(u => u.PasswordHash, opt => opt.Ignore())
				.ForMember(u => u.Reviews, opt => opt.Ignore());

			// Review
			CreateMap<CreateReviewRequestDto, Review>()
				.ForMember(c => c.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(c => c.DateCreated, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(c => c.User, opt => opt.Ignore())
				.ForMember(c => c.Recipe, opt => opt.Ignore());

			CreateMap<UpdateReviewRequestDto, Review>()
				.ForMember(c => c.User, opt => opt.Ignore())
				.ForMember(c => c.Recipe, opt => opt.Ignore())
				.ForMember(c => c.DateCreated, opt => opt.Ignore());

			CreateMap<Review, ReviewDto>()
				.ForMember(rd => rd.UserName, opt => opt.MapFrom(r => r.User.UserName));

			// Recipe
			CreateMap<CreateRecipeRequestDto, Recipe>()
				.ForMember(r => r.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(r => r.RecipeIngredients, opt => opt.MapFrom(_ => new List<RecipeIngredient>()))
				.ForMember(r => r.Categories, opt => opt.Ignore());

			CreateMap<UpdateRecipeRequestDto, Recipe>()
				.ForMember(r => r.RecipeIngredients, opt => opt.MapFrom(_ => new List<RecipeIngredient>()))
				.ForMember(r => r.Categories, opt => opt.Ignore());

			CreateMap<Recipe, RecipeDto>()
				.ForMember(
					dto => dto.Rating,
					opt => opt.MapFrom(r => r.Reviews.Any()
						? Math.Round(r.Reviews.Average(review => review.Rating), 1)
						: 0
				))
				.ForMember(dto => dto.AuthorUserName, opt => opt.MapFrom(r => r.Author.UserName))
				.ForMember(dto => dto.Ingredients, opt => opt.MapFrom(r => r.RecipeIngredients.Select(ri => new IngredientQuantityDto(ri.Ingredient.Name, ri.Quantity, ri.MeasurementUnit))))
				.ForMember(dto => dto.CategoryIds, opt => opt.MapFrom(r => r.Categories.Select(c => c.Id)));
		}
	}
}
