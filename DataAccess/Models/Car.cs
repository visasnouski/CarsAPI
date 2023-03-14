namespace DataAccess.Models
{
	public class Car
	{
		public int CarId { get; set; }

		public string? TagNumber { get; set; }

		public string? Make { get; set; }

		public string Model { get; set; } = null!;

		public int? CarYear { get; set; }

		public string? Category { get; set; }

		public bool? Mp3Layer { get; set; }

		public bool? DvdPlayer { get; set; }

		public bool? AirConditioner { get; set; }

		public bool? Abs { get; set; }

		public bool? Asr { get; set; }

		public bool? Navigation { get; set; }

		public bool? Available { get; set; }
	}
}
