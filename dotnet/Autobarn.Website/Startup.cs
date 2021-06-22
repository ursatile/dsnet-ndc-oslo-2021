using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Messaging;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.Website {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddRouting(options => options.LowercaseUrls = true);
			services.AddControllersWithViews().AddNewtonsoftJson();
			services.AddRazorPages().AddRazorRuntimeCompilation();

			switch (Configuration["DatabaseMode"]) {
				case "sql":
					var sqlConnectionString = Configuration.GetConnectionString("AutobarnSqlConnectionString");
					services.UseAutobarnSqlDatabase(sqlConnectionString);
					break;
				default:
					services.AddSingleton<IAutobarnDatabase, AutobarnCsvFileDatabase>();
					break;
			}

			var busConnectionString = Configuration.GetConnectionString("AzureServiceBusConnectionString");
			services.AddSingleton(_ => new ServiceBusClient(busConnectionString));

			services.AddSingleton<IAutobarnServiceBus>(s => new AutobarnAzureServiceBus(
				s.GetRequiredService<ServiceBusClient>(),
				Configuration["ServiceBus:TopicName"]));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
