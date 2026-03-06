using Microsoft.EntityFrameworkCore;

namespace AirportModelsLibrary
{
	public class FlightDepartureContext : DbContext
	{
		public FlightDepartureContext(DbContextOptions<FlightDepartureContext> options) : base(options)
		{
		}

		public DbSet<FlightDeparture> FlightDepartures { get; set; } = null!;
	}
}
