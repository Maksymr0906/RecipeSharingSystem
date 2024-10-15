using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RecipeSharingSystem.Persistence
{
	public class AuthDbContext(DbContextOptions<AuthDbContext> options)
		: IdentityDbContext(options)
	{
	}
}
