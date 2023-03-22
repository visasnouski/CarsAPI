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

		public Task<IEnumerable<Car>> GetCars(CancellationToken cancellationToken) =>
			_dbAccess.LoadData<Car, dynamic>("dbo.spCars_GetAll", new { }, cancellationToken);

		public Task<IEnumerable<Car>> GetLimitedCars(int offset, int limit, CancellationToken cancellationToken) =>
			_dbAccess.LoadData<Car, dynamic>("dbo.spCars_GetPage", new { Offset = offset, Limit = limit }, cancellationToken);


		public async Task<Car?> GetCar(int id, CancellationToken cancellationToken)
		{
			var result = await _dbAccess.LoadData<Car, dynamic>("dbo.spCars_Get", new { CarId = id }, cancellationToken);
			return result.FirstOrDefault();
		}

		public Task InsertCar(Car car, CancellationToken cancellationToken) => _dbAccess.SaveData("dbo.spCars_Insert", car, cancellationToken);

		public Task UpdateCar(Car car, CancellationToken cancellationToken) => _dbAccess.SaveData("dbo.spCars_Update", car, cancellationToken);

		public Task DeleteCar(int id, CancellationToken cancellationToken) => _dbAccess.SaveData("dbo.spCars_Delete", new { CarId = id }, cancellationToken);
	}
}
