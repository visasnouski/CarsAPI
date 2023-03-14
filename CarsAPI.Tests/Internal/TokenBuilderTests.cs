using CarsAPI.Interfaces;
using CarsAPI.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarsAPI.Tests.Internal
{
	[TestClass]
	public class TokenBuilderTests
	{
		[TestMethod]
		public void CreateToken_Correct()
		{
			// Arrange

			var dateTimeMock = new Mock<IDateTimeFacade>();
			dateTimeMock.Setup(x => x.CurrentDateTime).Returns(new DateTime(2022, 11, 15));
			var tokenBuilder = new TokenBuilder(dateTimeMock.Object);

			// Act

			var result = tokenBuilder.CreateToken("SomeUserName", "my top secret key");

			// Assert

			Assert.AreEqual(
				"eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU29tZVVzZXJOYW1lIiwiZXhwIjoxNjY4NTQ2MDAwfQ.uXeBjUlZAVe2kF5OMu82_KA8xdmhObkA3oO58Z3BFxvSPeliVYqFOY8mBkbrWeXrJXBNfLamNgafZfNWbiYBEA",
				result);
		}
	}
}