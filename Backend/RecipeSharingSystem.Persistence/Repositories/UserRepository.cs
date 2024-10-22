using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Persistence.Repositories;

public class UserRepository(RecipeSharingSystemDbContext context)
	: AbstractRepository<User>(context), IUserRepository
{
	public async Task<User> GetByEmail(string email)
	{
		var user = await Entities.FirstOrDefaultAsync(x => x.Email == email);
		return user;
	}

	public async Task<HashSet<PermissionEnum>> GetPermissions(Guid userId)
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
			.SelectMany(role => role.RolePermissions.Select(rp => (PermissionEnum)rp.Permission.Id))
			.ToHashSet();

		return permissions;
	}
}
