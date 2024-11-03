namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    IReviewRepository ReviewRepository { get; }
    IImageRepository ImageRepository { get; }
    IIngredientRepository IngredientRepository { get; }
    IInstructionRepository InstructionRepository { get; }
    IRatingRepository RatingRepository { get; }
    IRecipeRepository RecipeRepository { get; }
    IUserRepository UserRepository { get; }
    Task SaveAsync();
}
