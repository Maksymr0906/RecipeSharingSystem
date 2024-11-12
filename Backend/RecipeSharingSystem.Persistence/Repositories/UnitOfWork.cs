using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RecipeSharingSystemDbContext _context;
    public UnitOfWork(RecipeSharingSystemDbContext context)
    {
        _context = context;
        CategoryRepository = new CategoryRepository(_context);
        ReviewRepository = new ReviewRepository(_context);
        ImageRepository = new ImageRepository(_context);
        IngredientRepository = new IngredientRepository(_context);
        InstructionRepository = new InstructionRepository(_context);
        RecipeRepository = new RecipeRepository(_context);
        UserRepository = new UserRepository(_context);
    }

    public UnitOfWork(
        RecipeSharingSystemDbContext context,
        ICategoryRepository categoryRepository,
        IReviewRepository reviewRepository,
        IImageRepository imageRepository,
        IIngredientRepository ingredientRepository,
        IInstructionRepository instructionRepository,
        IRecipeRepository recipeRepository,
        IUserRepository userRepository)
    {
        _context = context;
        CategoryRepository = categoryRepository;
		ReviewRepository = reviewRepository;
        ImageRepository = imageRepository;
        IngredientRepository = ingredientRepository;
        InstructionRepository = instructionRepository;
        RecipeRepository = recipeRepository;
        UserRepository = userRepository;
    }

    public ICategoryRepository CategoryRepository { get; set; }

    public IReviewRepository ReviewRepository { get; set; }

    public IImageRepository ImageRepository { get; set; }

    public IIngredientRepository IngredientRepository { get; set; }

    public IInstructionRepository InstructionRepository { get; set; }

    public IRecipeRepository RecipeRepository { get; set; }

    public IUserRepository UserRepository { get; set; }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
