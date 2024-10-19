﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecipeSharingSystem.Application.Interfaces;
using RecipeSharingSystem.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeSharingSystem.Infrastructure;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
	private readonly JwtOptions _options = options.Value;
	public string Generate(User user)
	{
		Claim[] claims = [new("userId", user.Id.ToString())];

		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
			SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			claims: claims,
			signingCredentials: signingCredentials,
			expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

		var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

		return tokenValue;
	}
}
