namespace RecipeSharingSystem.Business.DTOs.User
{
	public class CreateUserRequestDto
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public bool IsAdmin { get; set; }
	}
}
