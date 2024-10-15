using RecipeSharingSystem.Core.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecipeSharingSystemDbContext _context;
        public UnitOfWork(RecipeSharingSystemDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(_context);
            CommentRepository = new CommentRepository(_context);
            ImageRepository = new ImageRepository(_context);
            IngredientRepository = new IngredientRepository(_context);
            InstructionRepository = new InstructionRepository(_context);
            RatingRepository = new RatingRepository(_context);
            RecipeRepository = new RecipeRepository(_context);
            UserRepository = new UserRepository(_context);
        }

        public UnitOfWork(
            RecipeSharingSystemDbContext context,
            ICategoryRepository categoryRepository,
            ICommentRepository commentRepository,
            IImageRepository imageRepository,
            IIngredientRepository ingredientRepository,
            IInstructionRepository instructionRepository,
            IRatingRepository ratingRepository,
            IRecipeRepository recipeRepository,
            IUserRepository userRepository)
        {
            _context = context;
            CategoryRepository = categoryRepository;
            CommentRepository = commentRepository;
            ImageRepository = imageRepository;
            IngredientRepository = ingredientRepository;
            InstructionRepository = instructionRepository;
            RatingRepository = ratingRepository;
            RecipeRepository = recipeRepository;
            UserRepository = userRepository;
        }

        public ICategoryRepository CategoryRepository { get; set; }

        public ICommentRepository CommentRepository { get; set; }

        public IImageRepository ImageRepository { get; set; }

        public IIngredientRepository IngredientRepository { get; set; }

        public IInstructionRepository InstructionRepository { get; set; }

        public IRatingRepository RatingRepository { get; set; }

        public IRecipeRepository RecipeRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
