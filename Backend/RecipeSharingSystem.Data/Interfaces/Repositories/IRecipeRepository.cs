using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IRecipeRepository : IRepository<Recipe>
{
    Task<Recipe> GetWithDetailsByIdAsync(Guid id);
    Task<ICollection<Recipe>> GetAllWithDetailsAsync();
    Task<Recipe> GetBySlugAsync(string slug);
	Task<ICollection<Recipe>> SearchRecipesAsync(string[] searchTerms);

	/// <summary>Author of the recipe that uses the instruction, or null if the instruction is not attached to a recipe yet.</summary>
	Task<Guid?> GetAuthorIdByInstructionIdAsync(Guid instructionId);
}
