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
			if (HttpContext.Session.GetString(_sessionProperty) != null)
			{
				_departures = JsonSerializer.Deserialize<List<FlightDeparture>>(HttpContext.Session.GetString(_sessionProperty));

			}

			var existingDeparture = _departures.Where(
				existingDeparture => existingDeparture.FlightNumber == departure.FlightNumber
				&& existingDeparture.DepartureTime.Equals(departure.DepartureTime)).FirstOrDefault();
			if (existingDeparture == null)
			{
				_departures.Add(departure);
			}
			else
			{
				return;
			}

			HttpContext.Session.SetString(
				_sessionProperty,
				JsonSerializer.Serialize(_departures)
			);

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
