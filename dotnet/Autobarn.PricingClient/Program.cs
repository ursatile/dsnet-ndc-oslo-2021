using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.PricingEngine;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Autobarn.PricingClient {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private static Pricer.PricerClient grpcClient;
        static async Task Main(string[] args) {
            var amqp = config.GetConnectionString("AutobarnRabbitMQ");
            using var bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus; Listening for newVehicleMessages");
            using var channel = GrpcChannel.ForAddress(config["AutobarnGrpcServerAddress"]);
            grpcClient = new Pricer.PricerClient(channel);
            Console.WriteLine("Connected to gRPC!");
            var subscriberId = $"Autobarn.AuditLog@{Environment.MachineName}";
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(subscriberId, HandleNewVehicleMessage);
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        private static async Task HandleNewVehicleMessage(NewVehicleMessage message) {
            var priceRequest = new PriceRequest() {
                Color = message.Color,
                Manufacturer = message.ManufacturerName,
                Model = message.ModelName,
                Year = message.Year
            };
            var priceReply = await grpcClient.GetPriceAsync(priceRequest);
            Console.WriteLine($"Vehicle {message.Registration} costs {priceReply.Price} {priceReply.CurrencyCode}");
        }

        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
