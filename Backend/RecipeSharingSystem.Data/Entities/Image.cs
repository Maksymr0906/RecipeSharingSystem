using System.ComponentModel.DataAnnotations;

namespace RecipeSharingSystem.Core.Entities;

public class Image : BaseEntity
{
	[Required]
	public string FileName { get; set; } = string.Empty;

	[Required]
	public string FileExtension { get; set; } = string.Empty;

	[Required]
	public string Title { get; set; } = string.Empty;

	[Required]
	public string Url { get; set; } = string.Empty;

	[Required]
	public DateTime DateCreated { get; set; }
}
