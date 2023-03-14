using CarsAPI.Controllers;
using CarsAPI.Tests.Extensions;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq.AutoMock;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace CarsAPI.Tests.Controllers
{
	[TestClass]
	public class CarsControllerTests
	{
		[TestMethod]
		public async Task GetLimitedCars_IfStartIndexIsGreaterThanEndIndex_OffsetAndLimitUsedCorrect()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			carsData.Setup(x => x.GetLimitedCars(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<Car>()
				{ new() { CarId = 1 }, new() { CarId = 2 } });
			mocker.Use(carsData);
			var target = mocker.CreateInstance<CarsController>();

			// Act

			await target.GetLimitedCars(2, 0);

			// Assert

			carsData.Verify(x => x.GetLimitedCars(0, 2), Times.Once);
		}

		[TestMethod]
		public async Task GetLimitedCars_IfEndIndexIsGreaterThanStartIndex_OffsetAndLimitUsedCorrect()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			carsData.Setup(x => x.GetLimitedCars(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<Car>()
				{ new() { CarId = 1 }, new() { CarId = 2 } });
			mocker.Use(carsData);
			var target = mocker.CreateInstance<CarsController>();

			// Act

			await target.GetLimitedCars(0, 2);

			// Assert

			carsData.Verify(x => x.GetLimitedCars(0, 2), Times.Once);
		}

		[TestMethod]
		public async Task GetLimitedCars_IfCarsExists_ReturnCars()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			carsData.Setup(x => x.GetLimitedCars(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<Car>()
				{ new() { CarId = 1 }, new() { CarId = 2 } });
			mocker.Use(carsData);
			var target = mocker.CreateInstance<CarsController>();

			// Act

			var result = await target.GetLimitedCars(0, 2);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
			var carIds = result.GetObjectResultContent()?.Select(x => x.CarId).ToArray();
			CollectionAssert.AreEqual(new[] { 1, 2 }, carIds);
		}

		[TestMethod]
		public async Task GetLimitedCars_IfCarsNotExists_ReturnEmpty()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			carsData.Setup(x => x.GetLimitedCars(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<Car>());
			mocker.Use(carsData);
			var target = mocker.CreateInstance<CarsController>();

			// Act

			var result = await target.GetLimitedCars(0, 2);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
			var carIds = result.GetObjectResultContent()?.Count();
			Assert.AreEqual(0, carIds);
		}

		[TestMethod]
		public async Task GetCar_IfCarsNotExists_ReturnNotFound()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			carsData.Setup(x => x.GetCar(It.IsAny<int>()));
			mocker.Use(carsData);
			var target = mocker.CreateInstance<CarsController>();

			// Act

			var result = await target.GetCar(0);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
		}

		[TestMethod]
		public async Task GetCar_IfCarsExists_ReturnCar()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			carsData.Setup(x => x.GetCar(It.IsAny<int>()))
				.ReturnsAsync(new Car() { CarId = 1 });
			mocker.Use(carsData);
			var target = mocker.CreateInstance<CarsController>();

			// Act

			var result = await target.GetCar(0);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
			var car = result.GetObjectResultContent();
			Assert.AreEqual(1, car?.CarId);
		}

		[TestMethod]
		public async Task CreateCar_CallInsertCar()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			var car = new Car() { CarId = 1 };

			mocker.Use(carsData);
			var target = mocker.CreateInstance<CarsController>();

			// Act

			var result = await target.CreateCar(car);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(OkResult));
			carsData.Verify(x => x.InsertCar(car), Times.Once);
		}

		[TestMethod]
		public async Task DeleteCar_CallDeleteCar()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			mocker.Use(carsData);
			var target = mocker.CreateInstance<CarsController>();

			// Act

			var result = await target.DeleteCar(1);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(OkResult));
			carsData.Verify(x => x.DeleteCar(1), Times.Once);
		}

		[TestMethod]
		public async Task UpdateCar_CallUpdateCar()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			var car = new Car() { CarId = 1 };

			mocker.Use(carsData);
			var target = mocker.CreateInstance<CarsController>();

			// Act

			var result = await target.UpdateCar(car);

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(OkResult));
			carsData.Verify(x => x.UpdateCar(car), Times.Once);
		}

		[TestMethod]
		public async Task GetAllCars_IfCarsExists_ReturnCars()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			var cars = new[] { new Car() { CarId = 1 }, new Car() { CarId = 2 } };
			carsData.Setup(x => x.GetCars()).ReturnsAsync(cars);
			mocker.Use(carsData);

			var target = mocker.CreateInstance<CarsController>();

			// Act

			var result = await target.GetAllCars();

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
			var carIds = result.GetObjectResultContent()?.Select(x => x.CarId).ToArray();
			CollectionAssert.AreEqual(new[] { 1, 2 }, carIds);
		}

		[TestMethod]
		public async Task GetAllCars_IfCarsNotExists_ReturnEmpty()
		{
			// Arrange

			var mocker = new AutoMocker();
			var carsData = new Mock<ICarsData>();

			carsData.Setup(x => x.GetCars()).ReturnsAsync(Array.Empty<Car>());
			mocker.Use(carsData);

			var target = mocker.CreateInstance<CarsController>();

			// Act

			var result = await target.GetAllCars();

			// Assert

			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
			var size = result.GetObjectResultContent()?.Count();
			Assert.AreEqual(0, size);
		}
	}
}