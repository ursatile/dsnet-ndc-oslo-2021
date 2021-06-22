using System.Collections.Generic;
using System.Linq;
using Autobarn.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Autobarn.Data {
	public class AutobarnSqlDatabase : IAutobarnDatabase {
		private readonly AutobarnDbContext dbContext;

		public AutobarnSqlDatabase(AutobarnDbContext dbContext) => this.dbContext = dbContext;

		public IEnumerable<Vehicle> ListVehicles() => dbContext.Vehicles;

		public IEnumerable<Manufacturer> ListManufacturers() => dbContext.Manufacturers;

		public IEnumerable<Model> ListModels() => dbContext.Models;

		public Vehicle FindVehicle(string registration) => dbContext.Vehicles.FirstOrDefault(v => v.Registration == registration);

		public Model FindModel(string code) => dbContext.Models.Find(code);

		public Manufacturer FindManufacturer(string code) => dbContext.Manufacturers.Find(code);

		public void CreateVehicle(Vehicle vehicle) {
			dbContext.Vehicles.Add(vehicle);
			dbContext.SaveChanges();
		}

		public void UpdateVehicle(Vehicle vehicle) {
			var existing = FindVehicle(vehicle.Registration);
			if (existing == default) {
				dbContext.Vehicles.Add(vehicle);
			} else {
				dbContext.Entry(existing).CurrentValues.SetValues(vehicle);
			}
			dbContext.SaveChanges();
		}

		public void DeleteVehicle(Vehicle vehicle) {
			dbContext.Vehicles.Remove(vehicle);
			dbContext.SaveChanges();
		}
	}

	public static class IServiceCollectionExtensions {
		public static IServiceCollection UseAutobarnSqlDatabase(this IServiceCollection services, string sqlConnectionString) {
			var loggerFactory = LoggerFactory.Create(builder => {
				builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information);
				builder.AddConsole();
			});

			services.AddDbContext<AutobarnDbContext>(options => {
				options.UseLazyLoadingProxies();
				options.UseLoggerFactory(loggerFactory);
				options.UseSqlServer(sqlConnectionString);
			});
			services.AddScoped<IAutobarnDatabase, AutobarnSqlDatabase>();
			return services;
		}
	}
}