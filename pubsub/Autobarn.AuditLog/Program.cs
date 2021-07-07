using EasyNetQ;
using Autobarn.Messages;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Autobarn.AuditLog
{
    class Program
	{
		private static readonly IConfigurationRoot config = ReadConfiguration();

		private const string SUBSCRIBER_ID = "Autobarn.AuditLog";

		static async Task Main()
		{
			using var bus = RabbitHutch.CreateBus(config.GetConnectionString("AutobarnRabbitMQ"));
			Console.WriteLine($"Connected! Listening for {nameof(VehicleAddedMessage)} messages.");
			await bus.PubSub.SubscribeAsync<VehicleAddedMessage>(SUBSCRIBER_ID, HandleNewVehicleMessage);
			Console.ReadKey(true);
		}

		private static void HandleNewVehicleMessage(VehicleAddedMessage message)
		{
			var csv = $"{message.Registration},{message.Manufacturer},{message.ModelName},{message.Color},{message.Year},{message.ListedAtUtc:O}";
			Console.WriteLine(csv);
		}

		private static IConfigurationRoot ReadConfiguration()
		{
			return new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.Build();
		}
	}
}