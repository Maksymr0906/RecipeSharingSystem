using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmail(string email);
    Task<User?> FindByEmailAsync(string email);
    Task<bool> ExistsByEmailOrUserNameAsync(string? email, string? userName, Guid excludeUserId = default);
    Task<HashSet<PermissionType>> GetPermissions(Guid userId);
    Task<User> GetByIdWithDetails(Guid id);
}
