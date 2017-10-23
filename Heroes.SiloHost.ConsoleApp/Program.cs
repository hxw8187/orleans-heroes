﻿using Heroes.Grains;
using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using System;
using System.Threading.Tasks;

namespace Heroes.SiloHost.ConsoleApp
{
	public class Program
	{
		public static int Main(string[] args)
		{
			return RunMainAsync().Result;
		}

		private static async Task<int> RunMainAsync()
		{
			try
			{
				var host = await StartSilo();
				Console.WriteLine("Press Enter to terminate...");
				Console.ReadLine();

				await host.StopAsync();

				return 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return 1;
			}
		}

		private static async Task<ISiloHost> StartSilo()
		{
			var config = ClusterConfiguration.LocalhostPrimarySilo();
			config.AddMemoryStorageProvider();

			var builder = new SiloHostBuilder()
				.UseConfiguration(config)
				.AddApplicationPartsFromReferences(typeof(HeroGrain).Assembly)
				.ConfigureLogging(logging => logging.AddConsole());

			var host = builder.Build();
			await host.StartAsync();
			return host;
		}
	}
}