﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IWCRM.API.Data;
using IWCRM.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace IWCRM.API.Services
{
	public class ServiceToken
	{
		public static string GenerateToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(Settings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.Name.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
				}),
				Expires = DateTime.UtcNow.AddHours(2),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public static string GenerateToken(IEnumerable<Claim> claims)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(Settings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddHours(2),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public static string RefreshToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		public static ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
		{
			var tokenValidationParameters = new TokenValidationParameters()
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Secret)),
				ValidateLifetime = false
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out var securityToken);
			if (securityToken is not JwtSecurityToken jwtSecurityToken ||
				!jwtSecurityToken.Header.Alg.Equals(value: SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException(message: "Invalid token");

			return principal;

		}

		private static List<(string, string)> _refreshTokens = new();

		//public static void SaveRefreshToken( string username, string refreshToken )
		//{
		//	_refreshTokens.Add( new( username, refreshToken ) );
		//}
		//public static string GetRefreshToken( string username )
		//{
		//	return _refreshTokens.FirstOrDefault( x => x.Item1 == username ).Item2;
		//}

		public static void DeleteRefreshToken( DataContext context, string username, string refreshToken)
		{
            var user = context.User.Where( x => x.Username == username ).FirstOrDefault();
            if (user != null)
            {
                user.RefreshToken = string.Empty;
                user.AccessToken = string.Empty;
                context.User.Update( user );
                context.SaveChanges();
                context = null;
            }
        }

		public static void SaveRefreshToken( DataContext context, string username,string accessToken, string refreshToken )
		{
            var user = context.User.Where( x => x.Username == username ).FirstOrDefault();
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.AccessToken = accessToken;
                context.User.Update( user );
                context.SaveChanges();
				context.Dispose();
				context = null;
            }
        }

        public static string GetRefreshToken( DataContext context, string username )
        { 
            return context.User.Where( x => x.Username == username ).FirstOrDefault().RefreshToken;
        }
  

    }
}
