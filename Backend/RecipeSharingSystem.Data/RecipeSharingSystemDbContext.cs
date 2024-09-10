using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Data
{
	public class RecipeSharingSystemDbContext : DbContext
	{
		public RecipeSharingSystemDbContext()
		{
		}

		public RecipeSharingSystemDbContext(DbContextOptions<RecipeSharingSystemDbContext> options)
			: base(options) 
		{
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Instruction> Instructions { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
		public DbSet<User> Users { get; set; }
	}
}
