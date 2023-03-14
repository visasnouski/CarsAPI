using CarsAPI.Interfaces;

namespace CarsAPI.Internal
{
	internal class DateTimeFacade : IDateTimeFacade
	{
		public DateTime CurrentDateTime => DateTime.Now;
	}
}