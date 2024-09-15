namespace RecipeSharingSystem.Business.DTOs.Image
{
	public class ImageUploadModel
	{
		public string FileName { get; set; }
		public string FileExtension { get; set; }
		public string Title { get; set; }
		public string LocalPath { get; set; }
		public string UrlPath { get; set; }
		public byte[] FileContent { get; set; }
	}
}
