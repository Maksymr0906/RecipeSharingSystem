using RecipeSharingSystem.Persistence.SeedData.Models;

namespace RecipeSharingSystem.Persistence.SeedData;

public class SeedDataOptions
{
	public UserSeedData[] Users { get; init; } = [];
	public UserRoleSeedData[] UserRoles { get; init; } = [];
	public ImageSeedData[] Images { get; init; } = [];
	public CategorySeedData[] Categories { get; init; } = [];
	public IngredientSeedData[] Ingredients { get; init; } = [];
	public RecipeSeedData[] Recipes { get; init; } = [];
	public RecipeCategorySeedData[] RecipeCategories {  get; init; } = [];
	public InstructionSeedData[] Instructions { get; init; } = [];
	public RecipeIngredientSeedData[] RecipeIngredients { get; init; } = [];
	public RatingSeedData[] Ratings { get; init; } = [];
	public ReviewSeedData[] Reviews { get; init; } = [];
}