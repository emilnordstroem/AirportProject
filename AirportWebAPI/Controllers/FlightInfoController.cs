using System.Text.Json;
using AirportModelsLibrary;
using AirportWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirportWebAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class FlightInfoController : Controller
	{
		private Publisher publisher = new Publisher();

		private List<FlightDeparture> _departures = new List<FlightDeparture>();
		private string _sessionProperty = "Departures";

		[HttpPost("api/flights/")]
		public void RegisterFlightDeparture(FlightDeparture departure)
		{
			if (HttpContext.Session.GetString(_sessionProperty) == null)
			{
				HttpContext.Session.SetString(
					_sessionProperty,
					JsonSerializer.Serialize(_departures)
				);
			}
			else
			{
				_departures = JsonSerializer.Deserialize<List<FlightDeparture>>(HttpContext.Session.GetString(_sessionProperty));
			}

			if (!_departures.Contains(departure))
			{
				_departures.Add(departure);
			}

			publisher.PublishDepartures(_departures);
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
