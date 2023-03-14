using CarsAPI.Interfaces;

namespace CarsAPI.Internal
{
	public class DateTimeFacade : IDateTimeFacade
	{
		public DateTime CurrentDateTime => DateTime.Now;
	}
}