using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Persistence.Configurations;

namespace RecipeSharingSystem.Persistence
{
	public class RecipeSharingSystemDbContext(
		DbContextOptions<RecipeSharingSystemDbContext> options,
		IOptions<AuthorizationOptions> authOptions) 
		: DbContext(options)
	{
		private readonly AuthorizationOptions _authorizationOptions = authOptions.Value;

		public DbSet<Category> Categories { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Instruction> Instructions { get; set; }
		public DbSet<Rating> Ratings { get; set; }
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

			modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));

			modelBuilder.ApplyConfiguration(new UserConfiguration(authOptions.Value));

			modelBuilder.ApplyConfiguration(new UserRoleConfiguration(authOptions.Value));
		}
	}
}
