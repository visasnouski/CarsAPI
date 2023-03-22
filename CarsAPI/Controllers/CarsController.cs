using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CarsController : ControllerBase
	{
		private readonly ICarsData _carsData;

		public CarsController(ICarsData carsData)
		{
			_carsData = carsData ?? throw new ArgumentNullException(nameof(carsData));
		}

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<IEnumerable<Car>>> GetAllCars(CancellationToken cancellationToken)
		{
			var cars = await _carsData.GetCars(cancellationToken);
			return Ok(cars);
		}

		[HttpGet("Range")]
		[Authorize]
		public async Task<ActionResult<IEnumerable<Car>>> GetLimitedCars(int startIndex, int endIndex, CancellationToken cancellationToken)
		{
			var (offset, limit) = GetLimit(startIndex, endIndex);
			var cars = await _carsData.GetLimitedCars(offset, limit, cancellationToken);
			return Ok(cars);
		}

		private static (int offset, int limit) GetLimit(int firstItem, int lastItem)
		{
			return firstItem < lastItem ?
				(firstItem, lastItem - firstItem) :
				(lastItem, firstItem - lastItem);
		}

		[HttpGet("{carId:int}")]
		[Authorize]
		public async Task<ActionResult<Car>> GetCar(int carId, CancellationToken cancellationToken)
		{
			var car = await _carsData.GetCar(carId, cancellationToken);
			if (car is null)
			{
				return NotFound();
			}
			return Ok(car);
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Car>> CreateCar(Car car, CancellationToken cancellationToken)
		{
			await _carsData.InsertCar(car, cancellationToken);
			return Ok();
		}

		[HttpPut]
		[Authorize]
		public async Task<ActionResult<Car>> UpdateCar(Car car, CancellationToken cancellationToken)
		{
			await _carsData.UpdateCar(car, cancellationToken);
			return Ok();
		}

		[HttpDelete("{carId:int}")]
		[Authorize]
		public async Task<ActionResult<Car>> DeleteCar(int carId, CancellationToken cancellationToken)
		{
			await _carsData.DeleteCar(carId, cancellationToken);
			return Ok();
		}
	}
}
