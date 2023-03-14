using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data
{
	public class CarsData : ICarsData
	{
		private readonly ISqlDataAccess _dbAccess;
		public CarsData(ISqlDataAccess dbAccess)
		{
			_dbAccess = dbAccess ?? throw new ArgumentNullException(nameof(dbAccess));
		}

		public Task<IEnumerable<Car>> GetCars() =>
			_dbAccess.LoadData<Car, dynamic>("dbo.spCars_GetAll", new { });

		public Task<IEnumerable<Car>> GetLimitedCars(int offset, int limit) =>
			_dbAccess.LoadData<Car, dynamic>("dbo.spCars_GetPage", new { Offset = offset, Limit = limit });


		public async Task<Car?> GetCar(int id)
		{
			var result = await _dbAccess.LoadData<Car, dynamic>("dbo.spCars_Get", new { CarId = id });
			return result.FirstOrDefault();
		}

		public Task InsertCar(Car car) => _dbAccess.SaveData("dbo.spCars_Insert", car);

		public Task UpdateCar(Car car) => _dbAccess.SaveData("dbo.spCars_Update", car);

		public Task DeleteCar(int id) => _dbAccess.SaveData("dbo.spCars_Delete", new { CarId = id });
	}
}
