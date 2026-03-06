using System.Text.Json;
using AirportModelsLibrary;
using AirportWebAPI.Models;
using AirportWebAPI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AirportWebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FlightInfoController : Controller
	{
		private Publisher publisher = new Publisher();

		private List<FlightDeparture> _departures = new List<FlightDeparture>();
		private string _sessionProperty = "Departures";

		private async void SaveChanges()
		{
			HttpContext.Session.SetString(
				_sessionProperty,
				JsonSerializer.Serialize(_departures)
			);

			await publisher.PublishDepartures(_departures);
		}

		[HttpPost()]
		public IActionResult RegisterFlightDeparture(FlightDeparture departure)
		{
			if (HttpContext.Session.GetString(_sessionProperty) != null)
			{
				_departures = JsonSerializer.Deserialize<List<FlightDeparture>>(HttpContext.Session.GetString(_sessionProperty));
			}

			var existingDeparture = _departures.Where(
				existingDeparture => existingDeparture.FlightNumber == departure.FlightNumber
				&& existingDeparture.DepartureTime.Equals(departure.DepartureTime)).FirstOrDefault();
			if (existingDeparture != null)
			{
				return Conflict($"{departure.FlightNumber} {departure.DepartureTime} already exists.");
			}
			_departures.Add(departure);
			_departures = DepartureSort.SortByDepartureTime(_departures);
			SaveChanges();
			return Ok(departure);
		}

		[HttpPut("{id}")]
		public IActionResult UpdateFlightDeparture(long id, FlightDeparture departure)
		{
			if (HttpContext.Session.GetString(_sessionProperty) == null)
			{
				return Unauthorized("Session not found.");
			}
			_departures = JsonSerializer.Deserialize<List<FlightDeparture>>(
				HttpContext.Session.GetString(_sessionProperty)
			);

			FlightDeparture departureToBeChanged = _departures.Where(currentDeparture => currentDeparture.Id == id).FirstOrDefault();
			if (departureToBeChanged == null)
			{
				return NotFound($"Departure with ID {id} not found.");
			}
			_departures.Remove(departureToBeChanged);
			_departures.Add(departure);
			_departures = DepartureSort.SortByDepartureTime(_departures);
			SaveChanges();

			return Ok(departure);
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteFlightDeparture(long id)
		{
			if (HttpContext.Session.GetString(_sessionProperty) == null)
			{
				return Unauthorized("Session not found.");
			}
			_departures = JsonSerializer.Deserialize<List<FlightDeparture>>(
				HttpContext.Session.GetString(_sessionProperty)
			);

			FlightDeparture departureToBeDeleted = _departures.Where(currentDeparture => currentDeparture.Id == id).FirstOrDefault();
			if (departureToBeDeleted == null)
			{
				return NotFound($"Departure with ID {id} not found.");
			}
			_departures.Remove(departureToBeDeleted);
			_departures = DepartureSort.SortByDepartureTime(_departures);
			SaveChanges();

			return Ok(departureToBeDeleted);
		}
	}
}
