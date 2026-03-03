using AirportModelsLibrary;

namespace AirportWebAPI.Utilities
{
	public class DepartureSort
	{
		public static List<FlightDeparture> SortByDepartureTime(List<FlightDeparture> departures)
		{
			return departures.OrderBy(departure => departure.DepartureTime).ToList();
		}
	}
}
