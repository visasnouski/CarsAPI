using CarsAPI.Controllers;
using CarsAPI.Interfaces;
using CarsAPI.Models;
using CarsAPI.Options;
using CarsAPI.Tests.Extensions;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;

namespace CarsAPI.Tests.Controllers
{
	[TestClass]
	public class AuthControllerTests
	{
		[TestMethod]
		public async Task Register_CallInsertUserCorrect()
		{
			// Arrange

			var mocker = new AutoMocker();
			var passwordManagerMock = new Mock<IPasswordManager>();
			var userDataMock = new Mock<IUserData>();

			var userDto = new UserDto() { UserName = "SomeUser", Password = "SomePassword" };

			var passwordHash = new byte[] { 1, 2 };
			var passwordSalt = new byte[] { 9, 10 };

			passwordManagerMock.Setup(x => x.GeneratePasswordHashAndSalt("SomePassword"))
				.Returns((passwordHash, passwordSalt));

			mocker.Use(passwordManagerMock);
			mocker.Use(userDataMock);

			var jwtOptions = Microsoft.Extensions.Options.Options.Create(new JwtSettings() { Secret = "SomeSecret" });
			mocker.Use(jwtOptions);

			var target = mocker.CreateInstance<AuthController>();

			// Act

			var result = await target.Register(userDto);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(OkResult));
			userDataMock.Verify(x => x.InsertUser(It.Is<User>(user => user.PasswordHash == passwordHash
																	  && user.PasswordSalt == passwordSalt
																	  && user.UserName == "SomeUser")), Times.Once);
		}

		[TestMethod]
		public async Task Login_IfUserNotExist_ReturnNotFoundResult()
		{
			// Arrange

			var mocker = new AutoMocker();
			var passwordManagerMock = new Mock<IPasswordManager>();
			var userDataMock = new Mock<IUserData>();

			var userDto = new UserDto() { UserName = "SomeUser", Password = "SomePassword" };

			mocker.Use(passwordManagerMock);
			mocker.Use(userDataMock);

			var jwtOptions = Microsoft.Extensions.Options.Options.Create(new JwtSettings() { Secret = "SomeSecret" });
			mocker.Use(jwtOptions);

			var target = mocker.CreateInstance<AuthController>();

			// Act

			var result = await target.Login(userDto);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
			Assert.AreEqual("User not found.", result.GetObjectResultContent());
			passwordManagerMock.Verify(x =>
				x.VerifyPasswordHashAndSalt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Never);

		}

		[TestMethod]
		public async Task Login_IfUserExistAndUsernameDoesNotMatch_ReturnNotFoundResult()
		{
			// Arrange

			var mocker = new AutoMocker();
			var passwordManagerMock = new Mock<IPasswordManager>();
			var userDataMock = new Mock<IUserData>();

			var userDto = new UserDto() { UserName = "SomeUser", Password = "SomePassword" };

			var passwordHash = new byte[] { 1, 2 };
			var passwordSalt = new byte[] { 9, 10 };

			userDataMock.Setup(x => x.GetUser(userDto.UserName)).ReturnsAsync(new User
			{ UserName = "someUser", PasswordSalt = passwordSalt, PasswordHash = passwordHash });

			mocker.Use(passwordManagerMock);
			mocker.Use(userDataMock);

			var jwtOptions = Microsoft.Extensions.Options.Options.Create(new JwtSettings() { Secret = "SomeSecret" });
			mocker.Use(jwtOptions);

			var target = mocker.CreateInstance<AuthController>();

			// Act

			var result = await target.Login(userDto);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
			Assert.AreEqual("User not found.", result.GetObjectResultContent());
			passwordManagerMock.Verify(x =>
				x.VerifyPasswordHashAndSalt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Never);

		}

		[TestMethod]
		public async Task Login_IfUserExistAndPasswordDoesNotMatch_ReturnUnauthorizedObjectResult()
		{
			// Arrange

			var mocker = new AutoMocker();
			var passwordManagerMock = new Mock<IPasswordManager>();
			var userDataMock = new Mock<IUserData>();
			var tokenBuilderMock = new Mock<ITokenBuilder>();

			var userDto = new UserDto() { UserName = "SomeUser", Password = "SomePassword" };

			var passwordHash = new byte[] { 1, 2 };
			var passwordSalt = new byte[] { 9, 10 };

			userDataMock.Setup(x => x.GetUser(userDto.UserName)).ReturnsAsync(new User
			{ UserName = "SomeUser", PasswordSalt = passwordSalt, PasswordHash = passwordHash });

			passwordManagerMock.Setup(x => x.VerifyPasswordHashAndSalt("SomePassword", passwordHash, passwordSalt))
				.Returns(false);

			mocker.Use(passwordManagerMock);
			mocker.Use(userDataMock);
			mocker.Use(tokenBuilderMock);

			var jwtOptions = Microsoft.Extensions.Options.Options.Create(new JwtSettings() { Secret = "SomeSecret" });
			mocker.Use(jwtOptions);

			var target = mocker.CreateInstance<AuthController>();

			// Act

			var result = await target.Login(userDto);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedObjectResult));
			Assert.AreEqual("Wrong password.", result.GetObjectResultContent());
			tokenBuilderMock.Verify(x =>
				x.CreateToken(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
		}

		[TestMethod]
		public async Task Login_IfUserExist_ReturnToken()
		{
			// Arrange

			var mocker = new AutoMocker();
			var passwordManagerMock = new Mock<IPasswordManager>();
			var userDataMock = new Mock<IUserData>();
			var tokenBuilderMock = new Mock<ITokenBuilder>();

			var userDto = new UserDto() { UserName = "SomeUser", Password = "SomePassword" };

			var passwordHash = new byte[] { 1, 2 };
			var passwordSalt = new byte[] { 9, 10 };

			userDataMock.Setup(x => x.GetUser(userDto.UserName)).ReturnsAsync(new User
			{ UserName = "SomeUser", PasswordSalt = passwordSalt, PasswordHash = passwordHash });

			passwordManagerMock.Setup(x => x.VerifyPasswordHashAndSalt("SomePassword", passwordHash, passwordSalt))
				.Returns(true);

			tokenBuilderMock.Setup(x => x.CreateToken("SomeUser", "SomeSecret")).Returns("SomeToken");

			mocker.Use(passwordManagerMock);
			mocker.Use(userDataMock);
			mocker.Use(tokenBuilderMock);

			var jwtOptions = Microsoft.Extensions.Options.Options.Create(new JwtSettings() { Secret = "SomeSecret" });
			mocker.Use(jwtOptions);

			var target = mocker.CreateInstance<AuthController>();

			// Act

			var result = await target.Login(userDto);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
			Assert.AreEqual("SomeToken", result.GetObjectResultContent());
		}
	}
}