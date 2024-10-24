using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IUserRepository : IAbstractRepository<User>
{
    Task<User> GetByEmail(string email);
    Task<HashSet<PermissionType>> GetPermissions(Guid userId);
}
