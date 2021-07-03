using System;

namespace Autobarn.Messages {
	public class NewCarMessage {
		public string Registration { get; set; }
		public string Manufacturer { get; set; }
		public string Model { get; set; }
		public string Color { get; set; }
		public int Year { get; set; }
		public DateTime ListedAtUtc { get; set; }
	}
}
