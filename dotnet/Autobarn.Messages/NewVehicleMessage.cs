using System;

namespace Autobarn.Messages {
    public class NewVehicleMessage {
        public string Registration { get; set; }
        public string ModelName { get; set; }
        public string ManufacturerName { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public DateTimeOffset CreatedAt { get; set;  }
    }
}
