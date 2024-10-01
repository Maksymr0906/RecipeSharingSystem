namespace RecipeSharingSystem.Data.Entities
{
	public class Image : BaseEntity
	{
		public string FileName { get; set; }
		public string FileExtension { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public DateTime DateCreated { get; set; }
		public ICollection<Category> Categories { get; set; }
		public ICollection<Recipe> Recipes { get; set; }
	}
}
