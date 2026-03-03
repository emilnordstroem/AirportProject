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

		private void SaveChanges()
		{
			HttpContext.Session.SetString(
				_sessionProperty,
				JsonSerializer.Serialize(_departures)
			);

			publisher.PublishDepartures(_departures);
		}


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

			SaveChanges();
		}

		[HttpPut("api/flights/")]
		public void UpdateFlightDeparture(long id, FlightDeparture departure)
		{
			if (HttpContext.Session.GetString(_sessionProperty) == null)
			{
				return;
			}
			_departures = JsonSerializer.Deserialize<List<FlightDeparture>>(HttpContext.Session.GetString(_sessionProperty));

			FlightDeparture deparetureToBeChanged = _departures.Where(currentDeparture => currentDeparture.Id == id).FirstOrDefault();
			if (deparetureToBeChanged != null)
			{
				_departures.Remove(deparetureToBeChanged);
				_departures.Add(departure);
			}

			SaveChanges();
		}

		[HttpDelete("api/flights/")]
		public void DeleteFlightDeparture(long id)
		{
			throw new NotImplementedException();
		}
	}
}
