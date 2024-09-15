using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RecipeSharingSystem.Business;
using RecipeSharingSystem.Business.Services.Implementation;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data;
using RecipeSharingSystem.Data.Repositories.Implementation;
using RecipeSharingSystem.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(AutomapperProfile));
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IInstructionRepository, InstructionRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IInstructionService, InstructionService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RecipeSharingSystemDbContext>(options =>
{
	options.UseMySql(builder.Configuration.GetConnectionString("RecipeSharingSystemConnectionString"), new MySqlServerVersion(new Version(8, 3, 0)));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
	RequestPath = "/Images"
});

app.MapControllers();

app.Run();
