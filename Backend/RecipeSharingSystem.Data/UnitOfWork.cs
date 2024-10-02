using RecipeSharingSystem.Data.Repositories.Implementation;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly RecipeSharingSystemDbContext _context;
		public UnitOfWork(RecipeSharingSystemDbContext context)
		{
			_context = context;
			this.CategoryRepository = new CategoryRepository(_context);
			this.CommentRepository = new CommentRepository(_context);
			this.ImageRepository = new ImageRepository(_context);
			this.IngredientRepository = new IngredientRepository(_context);
			this.InstructionRepository = new InstructionRepository(_context);
			this.RatingRepository = new RatingRepository(_context);
			this.RecipeRepository = new RecipeRepository(_context);
			this.UserRepository = new UserRepository(_context);
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
