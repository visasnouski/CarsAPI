using CarsAPI.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Text;

namespace CarsAPI.Tests.Internal
{
	[TestClass]
	public class PasswordManagerTests
	{
		[TestMethod]
		public void GeneratePasswordHashAndSalt_passwordHashIsCorrect()
		{
			// Arrange

			var passwordManager = new PasswordManager();

			// Act

			var (passwordHash, passwordSalt) = passwordManager.GeneratePasswordHashAndSalt("1234");

			// Assert

			using var hmac = new HMACSHA512(passwordSalt);
			var expectedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("1234"));

			CollectionAssert.AreEqual(expectedHash, passwordHash);
		}

		[TestMethod]
		public void VerifyPasswordHashAndSalt_IfCorrect_True()
		{
			// Arrange

			var passwordManager = new PasswordManager();
			var (expectedHash, expectedSalt) = GeneratePasswordHashAndSalt("1234");

			// Act

			var result = passwordManager.VerifyPasswordHashAndSalt("1234", expectedHash, expectedSalt);

			// Assert

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void VerifyPasswordHashAndSalt_IfSaltIsIncorrect_False()
		{
			// Arrange

			var passwordManager = new PasswordManager();
			var (expectedHash, _) = GeneratePasswordHashAndSalt("1234");

			// Act

			var result = passwordManager.VerifyPasswordHashAndSalt("1234", expectedHash, new byte[] { 1, 2 });

			// Assert

			Assert.IsFalse(result);
		}

		private static (byte[] passwordHash, byte[] passwordSalt) GeneratePasswordHashAndSalt(string password)
		{
			using var hmac = new HMACSHA512();
			var passwordSalt = hmac.Key;
			var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

			return (passwordHash, passwordSalt);
		}
	}
}