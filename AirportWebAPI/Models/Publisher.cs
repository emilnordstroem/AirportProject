using System.Text;
using System.Text.Json;
using AirportModelsLibrary;
using RabbitMQ.Client;

namespace AirportWebAPI.Models
{
	public class Publisher
	{
		// Pædagogisk tip: Brug altid 'async Task' i stedet for 'async void' (undtagen ved event handlers),
		// så fejlene kan fanges og kaldet kan afventes (awaited).
		public async Task PublishDepartures(List<FlightDeparture> departure)
		{
			var factory = new ConnectionFactory { HostName = "localhost" };
			using var connection = await factory.CreateConnectionAsync();
			using var channel = await connection.CreateChannelAsync();

			await channel.ExchangeDeclareAsync(
				exchange: "flightdepartures",
				type: ExchangeType.Fanout
			);

			var json = JsonSerializer.Serialize<List<FlightDeparture>>(departure);
			var body = Encoding.UTF8.GetBytes(json);

			await channel.BasicPublishAsync(
				exchange: "flightdepartures",
				routingKey: string.Empty,
				body: body
			);
			Console.WriteLine($" [x] Sent {departure.ToString()}");

			Console.WriteLine(" Press [enter] to exit.");
			Console.ReadLine();
		}
	}
}
