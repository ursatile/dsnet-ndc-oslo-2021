using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Autobarn.AuditLog {
	class Program {
		private static readonly IConfigurationRoot config = ReadConfiguration();

		private const string SUBSCRIBER_ID = "Autobarn.AuditLog";

		static async Task Main(string[] args) {
			using var bus = RabbitHutch.CreateBus(config.GetConnectionString("rabbitmq"));
			Console.WriteLine("Connected! Listening for NewCarMessage messages.");
			await bus.PubSub.SubscribeAsync<NewCarMessage>(SUBSCRIBER_ID, HandleNewCarMessage);
			Console.ReadKey(true);
		}

		private static void HandleNewCarMessage(NewCarMessage message) {
			var csv =
				$"{message.Registration},{message.Manufacturer},{message.Model},{message.Color},{message.Year},{message.ListedAtUtc:O}";
			Console.WriteLine(csv);
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
