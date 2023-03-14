using CarsAPI.Interfaces;
using CarsAPI.Model;
using CarsAPI.Options;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CarsAPI.Controllers
{
	[Microsoft.AspNetCore.Components.Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserData _userData;
		private readonly ITokenBuilder _tokenBuilder;
		private readonly IPasswordManager _passwordManager;
		private readonly JwtSettings _jwtSettings;

		public AuthController(IUserData userData, ITokenBuilder tokenBuilder, IPasswordManager passwordManager,
			IOptions<JwtSettings> jwtSettings)
		{
			_userData = userData ?? throw new ArgumentNullException(nameof(userData));
			_tokenBuilder = tokenBuilder ?? throw new ArgumentNullException(nameof(tokenBuilder));
			_passwordManager = passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
			_jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
		}

		[HttpPost("register")]
		[AllowAnonymous]
		public async Task<ActionResult<User>> Register(UserDto request)
		{
            (byte[] passwordHash, byte[] passwordSalt) = _passwordManager.GeneratePasswordHashAndSalt(request.Password);

			var user = new User
			{ UserName = request.UserName, PasswordHash = passwordHash, PasswordSalt = passwordSalt };
			await _userData.InsertUser(user);

			return Ok();
		}

		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<ActionResult<string>> Login(UserDto request)
		{
            var user = await _userData.GetUser(request.UserName);

			if (user?.UserName != request.UserName)
			{
				return NotFound("User not found.");
			}

			if (!_passwordManager.VerifyPasswordHashAndSalt(request.Password, user.PasswordHash, user.PasswordSalt))
			{
				return Unauthorized("Wrong password.");
			}

			var token = _tokenBuilder.CreateToken(user.UserName, _jwtSettings.Secret!);
			return Ok(token);
		}
	}
}