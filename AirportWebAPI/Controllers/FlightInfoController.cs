using AirportModelsLibrary;
using AirportWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirportWebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FlightInfoController : Controller
	{
		private readonly FlightDepartureContext _context;
		private readonly Publisher _publisher;

		public FlightInfoController(FlightDepartureContext context, Publisher publisher)
		{
			_context = context;
			_publisher = publisher;
		}

		private async Task SaveChanges()
		{
			var sorted = _context.FlightDepartures.OrderBy(departure => departure.DepartureTime);
			await _context.SaveChangesAsync();
			await _publisher.PublishDepartures(sorted.ToList());
		}

		[HttpPost()]
		public async Task<IActionResult> RegisterFlightDeparture(FlightDeparture departure)
		{
			_context.FlightDepartures.Add(departure);
			await SaveChanges();
			return Ok(departure);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateFlightDeparture(long id, FlightDeparture departure)
		{
			_context.FlightDepartures.Update(departure);
			await SaveChanges();
			return Ok(departure);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFlightDeparture(long id)
		{
			var departureToBeDeleted = _context.FlightDepartures.Find(id);
			_context.FlightDepartures.Remove(departureToBeDeleted);
			await SaveChanges();
			return Ok(departureToBeDeleted);
		}
	}
}
