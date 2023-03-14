using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarsAPI.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CarsAPI.Internal
{
	internal class TokenBuilder : ITokenBuilder

	{
		private readonly IDateTimeFacade _dateTimeFacade;

		public TokenBuilder(IDateTimeFacade dateTimeFacade)
		{
			_dateTimeFacade = dateTimeFacade ?? throw new ArgumentNullException(nameof(dateTimeFacade));
		}

		public string CreateToken(string userName, string secret)
		{
			var claims = new List<Claim>
			{
				new(ClaimTypes.Name, userName)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
			var token = new JwtSecurityToken(claims: claims, expires: _dateTimeFacade.CurrentDateTime.AddDays(1), signingCredentials: cred);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
	}
}
