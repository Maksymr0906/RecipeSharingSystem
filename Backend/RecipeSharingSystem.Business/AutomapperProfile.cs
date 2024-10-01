﻿using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Category;
using RecipeSharingSystem.Business.DTOs.Comment;
using RecipeSharingSystem.Business.DTOs.Image;
using RecipeSharingSystem.Business.DTOs.Ingredient;
using RecipeSharingSystem.Business.DTOs.Instruction;
using RecipeSharingSystem.Business.DTOs.Rating;
using RecipeSharingSystem.Business.DTOs.Recipe;
using RecipeSharingSystem.Business.DTOs.User;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Business
{
	public class AutomapperProfile : Profile
	{
		public AutomapperProfile() 
		{
			// Category
			CreateMap<CreateCategoryRequestDto, Category>()
				.ForMember(c => c.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(c => c.Image, opt => opt.Ignore());

			CreateMap<UpdateCategoryRequestDto, Category>()
				.ForMember(c => c.Image, opt => opt.Ignore());

			CreateMap<Category, CategoryDto>();

			// Image
			CreateMap<ImageUploadModel, Image>()
				.ForMember(i => i.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(i => i.DateCreated, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(i => i.Url, opt => opt.MapFrom(im => im.UrlPath))
				.ForMember(i => i.Categories, opt => opt.Ignore())
				.ForMember(i => i.Recipes, opt => opt.Ignore());

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
			CreateMap<CreateUserRequestDto, User>()
				.ForMember(u => u.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(u => u.Comments, opt => opt.Ignore())
				.ForMember(u => u.Ratings, opt => opt.Ignore());

			CreateMap<UpdateUserRequestDto, User>()
				.ForMember(u => u.Comments, opt => opt.Ignore())
				.ForMember(u => u.Ratings, opt => opt.Ignore());

			CreateMap<User, UserDto>()
				.ForMember(dto => dto.CommentIds, opt => opt.MapFrom(u => u.Comments.Select(c => c.Id)))
				.ForMember(dto => dto.RatingIds, opt => opt.MapFrom(u => u.Ratings.Select(r => r.Id)));

			// Comment
			CreateMap<CreateCommentRequestDto, Comment>()
				.ForMember(c => c.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(c => c.DateCreated, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(c => c.User, opt => opt.Ignore())
				.ForMember(c => c.Recipe, opt => opt.Ignore());

			CreateMap<UpdateCommentRequestDto, Comment>()
				.ForMember(c => c.User, opt => opt.Ignore())
				.ForMember(c => c.Recipe, opt => opt.Ignore())
				.ForMember(c => c.DateCreated, opt => opt.Ignore());

			CreateMap<Comment, CommentDto>();

			// Rating
			CreateMap<CreateRatingRequestDto, Rating>()
				.ForMember(r => r.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(r => r.User, opt => opt.Ignore())
				.ForMember(r => r.Recipe, opt => opt.Ignore());

			CreateMap<UpdateRatingRequestDto, Rating>()
				.ForMember(r => r.User, opt => opt.Ignore())
				.ForMember(r => r.Recipe, opt => opt.Ignore());

			CreateMap<Rating, RatingDto>();

			// Recipe
			CreateMap<CreateRecipeRequestDto, Recipe>()
				.ForMember(r => r.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
				.ForMember(r => r.Ratings, opt => opt.Ignore())
				.ForMember(r => r.RecipeIngredients, opt => opt.MapFrom((dto, recipe) =>
					dto.Ingredients.Select(iq => new RecipeIngredient
					{
						IngredientId = iq.IngredientId,
						RecipeId = recipe.Id,
						Quantity = iq.Quantity,
						MeasurementUnit = iq.MeasurementUnit,
					})
				))
				.ForMember(r => r.Categories, opt => opt.Ignore());

			CreateMap<UpdateRecipeRequestDto, Recipe>()
				.ForMember(r => r.Ratings, opt => opt.Ignore())
				.ForMember(r => r.RecipeIngredients, opt => opt.MapFrom((dto, recipe) =>
					dto.Ingredients.Select(iq => new RecipeIngredient
					{
						IngredientId = iq.IngredientId,
						RecipeId = recipe.Id,
						Quantity = iq.Quantity,
						MeasurementUnit = iq.MeasurementUnit,
					})
				))
				.ForMember(r => r.Categories, opt => opt.Ignore());

			CreateMap<Recipe, RecipeDto>()
				.ForMember(dto => dto.RatingIds, opt => opt.MapFrom(r => r.Ratings.Select(rt => rt.Id)))
				.ForMember(dto => dto.Ingredients, opt => opt.MapFrom(r =>
					r.RecipeIngredients.Select(ri => new IngredientQuantityDto
					{
						IngredientId = ri.IngredientId,
						Quantity = ri.Quantity,
						MeasurementUnit = ri.MeasurementUnit,
					})
				))
				.ForMember(dto => dto.CategoryIds, opt => opt.MapFrom(r => r.Categories.Select(c => c.Id)));
		}
	}
}
