using EasyNetQ;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;

namespace Autobarn.AuditLog {
    internal class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        static async Task Main(string[] args) {
            var amqp = config.GetConnectionString("AutobarnRabbitMQ");
            using var bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus! Listening for newVehicleMessages");
            var subscriberId = $"Autobarn.AuditLog@{Environment.MachineName}";
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(subscriberId, HandleNewVehicleMessage);
            Console.ReadLine();
        }

        private static void HandleNewVehicleMessage(NewVehicleMessage nvm) {
            var csvRow =
                $"{nvm.Registration},{nvm.ManufacturerName},{nvm.ModelName},{nvm.Year},{nvm.Color},{nvm.CreatedAt:O}";
            Console.WriteLine(csvRow);
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
