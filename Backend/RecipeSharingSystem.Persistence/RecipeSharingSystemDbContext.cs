using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.Configurations;
using RecipeSharingSystem.Persistence.SeedData;

namespace RecipeSharingSystem.Persistence
{
    public class RecipeSharingSystemDbContext(
		DbContextOptions<RecipeSharingSystemDbContext> options,
		IOptions<AuthorizationOptions> authOptions,
		IOptions<SeedDataOptions> seedDataOptions) 
		: DbContext(options)
	{
		private readonly AuthorizationOptions _authorizationOptions = authOptions.Value;
		private readonly SeedDataOptions _seedDataOptions = seedDataOptions.Value;

		public DbSet<Category> Categories { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Instruction> Instructions { get; set; }
		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Permission> Permissions { get; set; }
		public DbSet<RolePermission> RolePermissions { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeSharingSystemDbContext).Assembly);

			modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(_authorizationOptions));

			modelBuilder.ApplyConfiguration(new UserConfiguration(_seedDataOptions));

			modelBuilder.ApplyConfiguration(new UserRoleConfiguration(_seedDataOptions));

			modelBuilder.ApplyConfiguration(new CategoryConfiguration(_seedDataOptions));

			modelBuilder.ApplyConfiguration(new ImageConfiguration(_seedDataOptions));

			modelBuilder.ApplyConfiguration(new IngredientConfiguration(_seedDataOptions));

			modelBuilder.ApplyConfiguration(new InstructionConfiguration(_seedDataOptions));

			modelBuilder.ApplyConfiguration(new RecipeConfiguration(_seedDataOptions));

			modelBuilder.ApplyConfiguration(new RecipeIngredientConfiguration(_seedDataOptions));

			modelBuilder.ApplyConfiguration(new ReviewConfiguration(_seedDataOptions));
		}
	}
}
