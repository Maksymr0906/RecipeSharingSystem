using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class UserRepository(RecipeSharingSystemDbContext context)
	: Repository<User>(context), IUserRepository
{
	public async Task<User> GetByEmail(string email)
	{
		var user = await Entities
			.Include(x => x.FavoriteRecipes)
			.Include(x => x.UserRoles)
				.ThenInclude(x => x.Role)
			.FirstOrDefaultAsync(x => x.Email == email);
		
		if (user == null)
		{
			throw new KeyNotFoundException();
		}
		
		return user;
	}

	public async Task<HashSet<PermissionType>> GetPermissions(Guid userId)
	{
		var userRoles = await Entities
			.AsNoTracking()
			.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
					.ThenInclude(r => r.RolePermissions)
						.ThenInclude(rp => rp.Permission)
			.Where(u => u.Id == userId)
			.SelectMany(u => u.UserRoles.Select(ur => ur.Role))
			.ToListAsync();

		var permissions = userRoles
			.SelectMany(role => role.RolePermissions.Select(rp => (PermissionType)rp.Permission.Id))
			.ToHashSet();

		return permissions;
	}

	public async Task<User> GetByIdWithDetails(Guid id)
	{
		var user = await Entities
			.Include(x => x.AuthoredRecipes)
			.Include(x => x.FavoriteRecipes)
			.Include(x => x.Reviews)
			.FirstOrDefaultAsync(x => x.Id == id);

		if (user == null)
		{
			throw new KeyNotFoundException();
		}

		return user;
	}
}
