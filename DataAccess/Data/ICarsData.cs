using DataAccess.Models;

namespace DataAccess.Data
{
	public interface ICarsData
	{
		Task<IEnumerable<Car>> GetCars();

		Task<Car?> GetCar(int id);

		Task InsertCar(Car car);

		Task UpdateCar(Car car);

		Task DeleteCar(int id);

		Task<IEnumerable<Car>> GetLimitedCars(int offset, int limit);
	}
}