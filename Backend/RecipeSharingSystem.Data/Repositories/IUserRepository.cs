using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Core.Repositories;

public interface IUserRepository : IAbstractRepository<User>
{
	Task<User> GetByEmail(string email);
}
