using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using RecipeSharingSystem.API.Extensions;
using RecipeSharingSystem.Application.Extensions;
using RecipeSharingSystem.Infrastructure;
using RecipeSharingSystem.Infrastructure.Auth;
using RecipeSharingSystem.Persistence;
using RecipeSharingSystem.Persistence.SeedData;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));
services.Configure<SeedDataOptions>(configuration.GetSection(nameof(SeedDataOptions)));

services.AddHttpContextAccessor();

services
	.AddPersistence(configuration)
	.AddApplication()
	.AddValidators()
	.AddInfrastructure();

services.AddApiAuthentication(configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Recipe Sharing System", Version = "v1" });

	// Configure JWT authentication for Swagger
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

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
	options.AllowAnyHeader();
	options.AllowAnyOrigin();
	options.AllowAnyMethod();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
	RequestPath = "/Images"
});

app.MapControllers();

app.Run();
