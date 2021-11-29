using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using ILogger = Castle.Core.Logging.ILogger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {

    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase {
        private readonly IAutobarnDatabase db;
        private readonly IBus bus;
        private readonly ILogger<VehiclesController> logger;

        public VehiclesController(IAutobarnDatabase db, 
            IBus bus,
            ILogger<VehiclesController> logger) {
            this.db = db;
            this.bus = bus;
            this.logger = logger;
        }

        // GET: api/vehicles
        [HttpGet]
        [Produces("application/hal+json")]
        public IActionResult Get(int index = 0, int count = 10) {
            var items = db.ListVehicles().Skip(index).Take(count).ToList()
                .Select(v => v.ToResource());
            var total = db.CountVehicles();
            var _links = HypermediaExtensions.Paginate("/api/vehicles", index, count, total);
            var _actions = new {
                create = new {
                    href = "/api/vehicles",
                    method = "POST",
                    type = "application/json",
                    name = "Create a new vehicle"
                }
            };
            var result = new {
                _links,
                _actions,
                total,
                index,
                count = items.Count(),
                items
            };
            return Ok(result);
        }

        // GET api/vehicles/ABC123
        [HttpGet("{id}")]
        [Produces("application/hal+json")]
        public IActionResult Get(string id) {
            var vehicle = db.FindVehicle(id);
            if (vehicle == default) return NotFound();
            var result = vehicle.ToResource();
            return Ok(result);
        }

        // POST api/vehicles
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VehicleDto dto) {
            var existing = db.FindVehicle(dto.Registration);
            if (existing != default)
                return Conflict(
                    $"Sorry - there is already a vehicle with registration {dto.Registration} in our system.");
            var vehicleModel = db.FindModel(dto.ModelCode);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel
            };
            db.CreateVehicle(vehicle);
            await PublishNewVehicleMessage(vehicle);
            logger.LogInformation($"Created new vehicle: {vehicle.Registration} ({vehicleModel?.Name} {vehicleModel?.Manufacturer?.Name}, {vehicle.Year})");
            return Created($"/api/vehicles/{dto.Registration}", vehicle.ToResource());
        }
        private async Task PublishNewVehicleMessage(Vehicle vehicle) {
            var message = vehicle.ToMessage();
            await bus.PubSub.PublishAsync(message);
        }

        // PUT api/vehicles/ABC123
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] VehicleDto dto) {
            var vehicleModel = db.FindModel(dto.ModelCode);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                ModelCode = vehicleModel.Code
            };
            db.UpdateVehicle(vehicle);
            return Ok(dto);
        }

        // DELETE api/vehicles/ABC123
        [HttpDelete("{id}")]
        public IActionResult Delete(string id) {
            var vehicle = db.FindVehicle(id);
            if (vehicle == default) return NotFound();
            db.DeleteVehicle(vehicle);
            return NoContent();
        }
    }
}
