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

		// TIP: Session-data er unik for hver bruger. I et rigtigt system bør alle se de samme fly,
		// så overvej en statisk liste (static) eller en fælles database-tabel i stedet.
		private async Task SaveChanges()
		{
			HttpContext.Session.SetString(
				_sessionProperty,
				JsonSerializer.Serialize(_departures)
			);

			await publisher.PublishDepartures(_departures);
		}


		[HttpPost()]
		public async Task<IActionResult> RegisterFlightDeparture(FlightDeparture departure)
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
			await SaveChanges();
			return Ok(departure);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateFlightDeparture(long id, FlightDeparture departure)
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
			await SaveChanges();

			return Ok(departure);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFlightDeparture(long id)
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
			await SaveChanges();

			return Ok(departureToBeDeleted);
		}
	}
}
