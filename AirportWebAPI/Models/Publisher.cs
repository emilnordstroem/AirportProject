using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AirportModelsLibrary;
using RabbitMQ.Client;

namespace AirportWebAPI.Models
{
	public class Publisher : IAsyncDisposable
	{
		private readonly IConnection _connection;
		private readonly IChannel _channel;

		// Private constructor — use CreateAsync() instead
		private Publisher(IConnection connection, IChannel channel)
		{
			_connection = connection;
			_channel = channel;
		}

		public static async Task<Publisher> Create()
		{
			var factory = new ConnectionFactory { HostName = "localhost" };
			var connection = await factory.CreateConnectionAsync();
			var channel = await connection.CreateChannelAsync();
			await channel.ExchangeDeclareAsync("flightdepartures", ExchangeType.Fanout);
			return new Publisher(connection, channel);
		}

		public async Task PublishDepartures(List<FlightDeparture> departures)
		{
			var json = JsonSerializer.Serialize(departures);
			var body = Encoding.UTF8.GetBytes(json);

			await _channel.BasicPublishAsync(
				exchange: "flightdepartures",
				routingKey: string.Empty,
				body: body
			);

			Console.WriteLine($" [x] Sent {departures.Count} departures");
		}

		public async ValueTask DisposeAsync()
		{
			await _channel.DisposeAsync();
			await _connection.DisposeAsync();
		}
	}
}