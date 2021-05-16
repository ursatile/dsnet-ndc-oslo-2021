using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autobarn.Data.Entities;

namespace Autobarn.Data {
	public interface IAutobarnDatabase {
		public IEnumerable<Vehicle> ListVehicles();
		public IEnumerable<Manufacturer> ListManufacturers();
		public IEnumerable<Model> ListModels();
		public Vehicle FindVehicle(string registration);
		public void AddVehicle(Vehicle vehicle);
	}

	public class AutobarnSqlDatabase : IAutobarnDatabase {
		private readonly AutobarnDbContext dbContext;

		public AutobarnSqlDatabase(AutobarnDbContext dbContext) {
			this.dbContext = dbContext;
		}

		public IEnumerable<Vehicle> ListVehicles() => dbContext.Vehicles;

		public IEnumerable<Manufacturer> ListManufacturers() => dbContext.Manufacturers;

		public IEnumerable<Model> ListModels() => dbContext.Models;

		public Vehicle FindVehicle(string registration) => dbContext.Vehicles.Find(registration);

		public void AddVehicle(Vehicle vehicle) {
			dbContext.Vehicles.Add(vehicle);
			dbContext.SaveChanges();
		}
	}
}
