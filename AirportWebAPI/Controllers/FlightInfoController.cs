using Microsoft.AspNetCore.Mvc;
using AirportModelsLibrary;

namespace AirportWebAPI.Controllers
{
	[ApiController]
	public class FlightInfoController : Controller
	{
		[HttpPost("api/flights/")]
		public void RegisterFlightDeparture(FlightDeparture departure)
		{
			throw new NotImplementedException();
		}

		[HttpPut("api/flights/")]
		public void UpdateFlightDeparture(long id, FlightDeparture departure)
		{
			throw new NotImplementedException();
		}

		[HttpDelete("api/flights/")]
		public void DeleteFlightDeparture(long id)
		{
			throw new NotImplementedException();
		}
	}
}
