namespace AirportModelsLibrary
{
	public class FlightDeparture
	{
		public static long _Id = 1;
		public long Id { get; set; }
		public DateTime DepartureTime { get; set; }
		public string FlightNumber { get; set; }
		public string Destination { get; set; }
		public DateTime BoardingTime { get; set; }
		public string Gate { get; set; }
		public Status Status { get; set; }

		public FlightDeparture() { }

		public FlightDeparture(long id, DateTime depareturetime, string flightnumber, string destination, DateTime boardingtime, string gate, Status status)
		{
			if (_Id <= id)
			{
				Id = id;
				_Id = ++id;
			}
			else
			{
				Id = _Id++;
			}
			DepartureTime = depareturetime;
			FlightNumber = flightnumber;
			Destination = destination;
			BoardingTime = boardingtime;
			Gate = gate;
			Status = status;
		}

		public override string ToString()
		{
			return $"{DepartureTime.TimeOfDay} | {FlightNumber} | {Destination} | {BoardingTime.TimeOfDay} | {Gate} | {Status.ToString()}";
		}
	}

	public enum Status
	{
		OnTime,
		Delayed,
		Boarding,
		Departed,
		Cancelled
	}
}
