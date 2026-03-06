using AirportModelsLibrary;
using AirportWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace AirportWebAPI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			var publisher = await Publisher.Create();
			builder.Services.AddSingleton(publisher);
			builder.Services.AddDbContext<FlightDepartureContext>(opt =>
				opt.UseInMemoryDatabase("Departures")
				);
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
			}

			app.UseAuthorization();
			app.MapControllers();
			app.MapScalarApiReference();

			app.Run();
		}
	}
}
