using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autobarn.Data;
using Autobarn.Data.Entities;

namespace Autobarn.Website.Controllers {
	public class VehiclesController : Controller {
		private readonly IAutobarnDatabase db;

		public VehiclesController(IAutobarnDatabase db) {
			this.db = db;
		}
		public IActionResult Index() {
			var vehicles = db.ListVehicles();
			return View(vehicles);
		}

		public IActionResult Details(string id) {
			var vehicle = db.FindVehicle(id);
			return View(vehicle);
		}
	}
}
