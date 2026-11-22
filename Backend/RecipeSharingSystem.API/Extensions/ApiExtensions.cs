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
					// Lab 6: a token must be scoped to a specific issuer/recipient, not merely
					// bear a valid signature - otherwise a token issued for another system that
					// happens to share (or recover) this signing key could be replayed here.
					ValidateIssuer = true,
					ValidIssuer = jwtOptions!.Issuer,
					ValidateAudience = true,
					ValidAudience = jwtOptions.Audience,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey =
					new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
					// Explicit, short, auditable allowance for clock drift between the token
					// issuer and this validator (both are the same API here, but the default
					// 5-minute skew is unnecessarily generous and was never a deliberate choice).
					ClockSkew = TimeSpan.FromSeconds(30)
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
			("CreateReviewPolicy", PermissionType.CreateReview),
			("UpdateRecipePolicy", PermissionType.UpdateRecipe),
			("UpdateIngredientPolicy", PermissionType.UpdateIngredient),
			("UpdateInstructionPolicy", PermissionType.UpdateInstruction),
			("UpdateReviewPolicy", PermissionType.UpdateReview),
			("DeleteRecipePolicy", PermissionType.DeleteRecipe),
			("DeleteInstructionPolicy", PermissionType.DeleteInstruction),
			("DeleteReviewPolicy", PermissionType.DeleteReview)
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
