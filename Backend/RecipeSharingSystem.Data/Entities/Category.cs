namespace RecipeSharingSystem.Data.Entities
{
	public class Category : BaseEntity
	{
		public string Name { get; set; }
		public Guid ImageId { get; set; }
		public Image Image { get; set; }
		public ICollection<Recipe> Recipes { get; set; }
	}
}
