using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autobarn.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autobarn.Data {
	public interface IAutobarnDatabase {
		public IEnumerable<Vehicle> ListVehicles();
		public IEnumerable<Manufacturer> ListManufacturers();
		public IEnumerable<Model> ListModels();
		public Vehicle FindVehicle(string registration);
		
		public Model FindModel(string code);
		public Manufacturer FindManufacturer(string code);
		
		public void AddVehicle(Vehicle vehicle);
	}

	public class AutobarnSqlDatabase : IAutobarnDatabase {
		private readonly AutobarnDbContext dbContext;

		public AutobarnSqlDatabase(AutobarnDbContext dbContext) {
			this.dbContext = dbContext;
		}

		public IEnumerable<Vehicle> ListVehicles() => dbContext.Vehicles;

		public IEnumerable<Manufacturer> ListManufacturers() => dbContext.Manufacturers
			.Include(m => m.Models)
			.ThenInclude(m => m.Vehicles);

		public IEnumerable<Model> ListModels() => dbContext.Models;

		public Vehicle FindVehicle(string registration) =>
			dbContext.Vehicles
				.Include(v => v.VehicleModel)
				.ThenInclude(vm => vm.Manufacturer)
				.FirstOrDefault(v => v.Registration == registration);

		public Model FindModel(string code) => dbContext.Models.Find(code);

		public Manufacturer FindManufacturer(string code) => dbContext.Manufacturers.Find(code);

		public void AddVehicle(Vehicle vehicle) {
			dbContext.Vehicles.Add(vehicle);
			dbContext.SaveChanges();
		}
	}
}
