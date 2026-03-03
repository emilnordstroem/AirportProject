using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using AirportModelsLibrary;

public class Program
{
	static async Task Main(string[] args)
	{
		var factory = new ConnectionFactory { HostName = "localhost" };
		using var connection = await factory.CreateConnectionAsync();
		using var channel = await connection.CreateChannelAsync();

		await channel.ExchangeDeclareAsync(
			exchange: "flightdepartures",
			type: ExchangeType.Fanout
		);

		QueueDeclareOk queueDeclareResult = await channel.QueueDeclareAsync();
		string queueName = queueDeclareResult.QueueName;
		await channel.QueueBindAsync(
			queue: queueName,
			exchange: "flightdepartures",
			routingKey: string.Empty
		);

		Console.WriteLine("Waiting for flight deparetures..");

		var consumer = new AsyncEventingBasicConsumer(channel);
		consumer.ReceivedAsync += async (model, ea) =>
		{
			byte[] body = ea.Body.ToArray();
			var json = Encoding.UTF8.GetString(body);
			var departures = JsonSerializer.Deserialize<List<FlightDeparture>>(json);
			if (departures == null)
			{
				return;
			}
			Console.WriteLine("==============================");
			Console.WriteLine("\\//\\//\\//\\//\\//\\//\\//\\");
			Console.WriteLine("==============================");
			foreach (var departure in departures)
			{
				Console.WriteLine(departure.ToString());
			}
			Console.WriteLine("==============================");
		};

		await channel.BasicConsumeAsync(
			queueName,
			autoAck: true,
			consumer: consumer
		);

		Console.ReadLine();
	}
}