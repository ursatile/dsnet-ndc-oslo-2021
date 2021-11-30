using EasyNetQ;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace Autobarn.Notifier {
    internal class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private static HubConnection hub;
        static async Task Main(string[] args) {
            var signalRHubUrl = config["AutobarnSignalRHubUrl"];
            hub = new HubConnectionBuilder().WithUrl(signalRHubUrl).Build();
            await hub.StartAsync();
            Console.WriteLine("SignalR Hub Started!");
            Console.WriteLine("Starting Autobarn.Notifier");
            var amqp = config.GetConnectionString("AutobarnRabbitMQ");
            using var bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus! Listening for newVehicleMessages");
            var subscriberId = $"Autobarn.Notifier@{Environment.MachineName}";
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>(subscriberId, HandleNewVehicleMessage);
            Console.ReadLine();
        }

        private static async void HandleNewVehicleMessage(NewVehiclePriceMessage nvpm) {
            var csvRow =
                $"{nvpm.Price} {nvpm.CurrencyCode} : {nvpm.Registration},{nvpm.ManufacturerName},{nvpm.ModelName},{nvpm.Year},{nvpm.Color},{nvpm.CreatedAt:O}";
            Console.WriteLine(csvRow);
            var json = JsonConvert.SerializeObject(nvpm);
            await hub.SendAsync("NotifyWebUsers", "Autobarn.Notifier",
                json);
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
