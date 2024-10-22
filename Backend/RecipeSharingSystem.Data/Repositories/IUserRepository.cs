using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Core.Repositories;

public interface IUserRepository : IAbstractRepository<User>
{
	Task<User> GetByEmail(string email);
	Task<HashSet<PermissionEnum>> GetPermissions(Guid userId);
}
