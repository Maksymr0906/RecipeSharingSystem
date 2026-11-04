using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using RecipeSharingSystem.API.Extensions;
using RecipeSharingSystem.Application.Extensions;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;
using RecipeSharingSystem.Infrastructure;
using RecipeSharingSystem.Infrastructure.Auth;
using RecipeSharingSystem.Infrastructure.Crypto;
using RecipeSharingSystem.Persistence;
using RecipeSharingSystem.Persistence.SeedData;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

// Secrets must not live in committed files: provide them with `dotnet user-secrets` or environment variables
// (see README.md). Fail fast with a clear message instead of starting with a weak or empty key.
var jwtSettings = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
if (jwtSettings is null || Encoding.UTF8.GetByteCount(jwtSettings.SecretKey) < 32)
{
	throw new InvalidOperationException(
		"JwtOptions:SecretKey is missing or shorter than 32 bytes. " +
		"Set it with `dotnet user-secrets set \"JwtOptions:SecretKey\" \"<random value>\"` or the JwtOptions__SecretKey environment variable.");
}

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<EncryptionOptions>(configuration.GetSection("Encryption"));
services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));
services.Configure<SeedDataOptions>(configuration.GetSection(nameof(SeedDataOptions)));

services.AddHttpContextAccessor();

services
	.AddPersistence(configuration)
	.AddApplication()
	.AddValidators()
	.AddInfrastructure();

services.AddApiAuthentication(configuration);

// Throttling of authentication endpoints: at most 5 requests per minute per IP address and endpoint
// (login and registration have separate counters). Behind a reverse proxy, configure forwarded headers
// first, otherwise every client appears to have the proxy's address.
services.AddRateLimiter(options =>
{
	options.OnRejected = async (context, cancellationToken) =>
	{
		context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
		await context.HttpContext.Response.WriteAsJsonAsync(
			new { title = "Too many requests.", detail = "Please wait a minute and try again." },
			cancellationToken);
	};

	options.AddPolicy("auth", httpContext =>
		RateLimitPartition.GetFixedWindowLimiter(
			$"{httpContext.Connection.RemoteIpAddress}|{httpContext.Request.Path}",
			_ => new FixedWindowRateLimiterOptions
			{
				PermitLimit = 5,
				Window = TimeSpan.FromMinutes(1),
				QueueLimit = 0
			}));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Recipe Sharing System", Version = "v1" });

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter JWT with Bearer into field",
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

var app = builder.Build();

// Fail fast on a missing or malformed encryption key ring (the encryptor is otherwise created lazily on first database use).
app.Services.GetRequiredService<IFieldEncryptor>();

// Key rotation helper: `dotnet run -- --reencrypt-pii` rewrites every encrypted personal-data field
// with the current key (Encryption:CurrentKeyId) and exits. See docs/security/cryptography.md.
if (args.Contains("--reencrypt-pii"))
{
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<RecipeSharingSystemDbContext>();

	var users = await db.Users.ToListAsync();
	foreach (var user in users)
	{
		var entry = db.Entry(user);
		entry.Property(u => u.FirstName).IsModified = true;
		entry.Property(u => u.LastName).IsModified = true;
		entry.Property(u => u.PostalCode).IsModified = true;
		entry.Property(u => u.DateOfBirth).IsModified = true;
	}

	await db.SaveChangesAsync();
	Console.WriteLine($"Re-encrypted personal data of {users.Count} user(s) with the current key.");
	return;
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Only the configured frontend origins may call the API from a browser (Cors:AllowedOrigins).
var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

app.UseCors(options =>
{
	options.WithOrigins(allowedOrigins);
	options.AllowAnyHeader();
	options.AllowAnyMethod();
});

// After CORS so that error responses keep their CORS headers; before everything that can throw.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
	RequestPath = "/Images"
});

app.MapControllers();

app.Run();
