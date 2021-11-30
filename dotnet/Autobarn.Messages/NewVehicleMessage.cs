using System;
using System.Threading;

namespace Autobarn.Messages {
    public class NewVehicleMessage {
        public string Registration { get; set; }
        public string ModelName { get; set; }
        public string ManufacturerName { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class NewVehiclePriceMessage : NewVehicleMessage {
        public int Price { get; set; }
        public string CurrencyCode { get; set; }

        public NewVehiclePriceMessage() {
        }

        public NewVehiclePriceMessage(NewVehicleMessage vehicle, int price, string currencyCode) {
            this.Registration = vehicle.Registration;
            this.Color = vehicle.Color;
            this.Year = vehicle.Year;
            this.ManufacturerName = vehicle.ManufacturerName;
            this.ModelName = vehicle.ModelName;
            this.Price = price;
            this.CurrencyCode = currencyCode;
        }
    }
}
