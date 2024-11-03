﻿namespace RecipeSharingSystem.Persistence.SeedData.Models;

public record ImageSeedData
{
	public required Guid Id { get; init; }
	public required string FileName { get; init; }
	public required string FileExtension { get; init; }
	public required string Title { get; init; }
	public required string Url { get; init; }
	public required DateTime DateCreated { get; init; } = DateTime.Now;
}
