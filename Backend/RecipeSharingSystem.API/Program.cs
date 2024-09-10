using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Data;

var builder = WebApplication.CreateBuilder(args);


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

app.MapControllers();

app.Run();
