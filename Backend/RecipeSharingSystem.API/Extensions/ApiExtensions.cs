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
			AddGenericPolicy(options, "CreatePolicy", PermissionType.Create);
			AddGenericPolicy(options, "ReadPolicy", PermissionType.Read);
			AddGenericPolicy(options, "UpdatePolicy", PermissionType.Update);
			AddGenericPolicy(options, "DeletePolicy", PermissionType.Delete);

			AddSpecificPolicies(options);
		});
	}

	private static void AddGenericPolicy(
		AuthorizationOptions options,
		string policyName,
		PermissionType permissionType)
	{
		options.AddPolicy(policyName, policy =>
		{
			policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
			policy.Requirements.Add(new PermissionRequirement([permissionType]));
		});
	}

	private static void AddSpecificPolicies(AuthorizationOptions options)
	{
		var specificPolicies = new[]
		{
			("CreateRecipePolicy", PermissionType.CreateRecipe),
			("CreateIngredientPolicy", PermissionType.CreateIngredient),
			("CreateInstructionPolicy", PermissionType.CreateInstruction),
			("UpdateRecipePolicy", PermissionType.UpdateRecipe),
			("UpdateIngredientPolicy", PermissionType.UpdateIngredient),
			("UpdateInstructionPolicy", PermissionType.UpdateInstruction),
			("DeleteRecipePolicy", PermissionType.DeleteRecipe),
			("DeleteInstructionPolicy", PermissionType.DeleteInstruction)
		};

		foreach (var (policyName, permissionType) in specificPolicies)
		{
			options.AddPolicy(policyName, policy =>
			{
				policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
				policy.Requirements.Add(new PermissionRequirement([permissionType]));
			});
		}
	}
}
