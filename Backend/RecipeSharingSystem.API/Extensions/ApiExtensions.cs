using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using RecipeSharingSystem.Core.Enums;
using RecipeSharingSystem.Infrastructure.Auth;
using System.Text;

namespace RecipeSharingSystem.API.Extensions;

public static class ApiExtensions
{
	public static void AddApiAuthentication(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					AuthenticationType = "Jwt",
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey =
					new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
				};
			});

		services.AddAuthorization();

		services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

		services.AddAuthorization(options =>
		{
			options.AddPolicy("CreatePolicy", policy =>
			{
				policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);

				policy.Requirements.Add(new PermissionRequirement([PermissionType.Create]));
			});

			options.AddPolicy("ReadPolicy", policy =>
			{
				policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);

				policy.Requirements.Add(new PermissionRequirement([PermissionType.Read]));
			});
			options.AddPolicy("UpdatePolicy", policy =>
			{
				policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);

				policy.Requirements.Add(new PermissionRequirement([PermissionType.Update]));
			});
			options.AddPolicy("DeletePolicy", policy =>
			{
				policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);

				policy.Requirements.Add(new PermissionRequirement([PermissionType.Delete]));
			});
		});
	}
}
