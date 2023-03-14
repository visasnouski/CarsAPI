using CarsAPI.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CarsAPI.Internal
{
	public class PasswordManager : IPasswordManager
	{
		public (byte[] passwordHash, byte[] passwordSalt) GeneratePasswordHashAndSalt(string password)
		{
			using var hmac = new HMACSHA512();
			var passwordSalt = hmac.Key;
			var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

			return (passwordHash, passwordSalt);
		}

		public bool VerifyPasswordHashAndSalt(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512(passwordSalt);
			var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			return computeHash.SequenceEqual(passwordHash);
		}
	}
}
