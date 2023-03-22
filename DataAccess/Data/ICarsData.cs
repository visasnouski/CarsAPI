using DataAccess.Models;

namespace DataAccess.Data
{
	public interface ICarsData
	{
		Task<IEnumerable<Car>> GetCars(CancellationToken cancellationToken);

		Task<Car?> GetCar(int id, CancellationToken cancellationToken);

		Task InsertCar(Car car, CancellationToken cancellationToken);

		Task UpdateCar(Car car, CancellationToken cancellationToken);

		Task DeleteCar(int id, CancellationToken cancellationToken);

		Task<IEnumerable<Car>> GetLimitedCars(int offset, int limit, CancellationToken cancellationToken);
	}
}