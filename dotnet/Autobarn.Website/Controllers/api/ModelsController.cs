using Autobarn.Data;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autobarn.Website.Models;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase {
        private readonly IAutobarnDatabase db;
        private readonly IBus bus;
        private readonly ILogger<ModelsController> logger;

        public ModelsController(IAutobarnDatabase db,
            IBus bus,
            ILogger<ModelsController> logger) {
            this.db = db;
            this.bus = bus;
            this.logger = logger;
        }
        [HttpGet]
        public IEnumerable<Model> Get() {
            return db.ListModels();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post(string id, [FromBody] VehicleDto dto) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound($"There is no vehicle model matching {dto.ModelCode} in our database");
            var existing = db.FindVehicle(dto.Registration);
            if (existing != default)
                return Conflict(
                    $"Sorry - there is already a vehicle with registration {dto.Registration} in our system.");
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


        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound();
            var result = vehicleModel.ToDynamic();
            result._actions = new {
                create = new {
                    href = $"/api/models/{id}",
                    method = "POST",
                    type = "application/json",
                    name = $"Create a new{vehicleModel.Manufacturer.Name} {vehicleModel.Name}"
                }
            };
            return Ok(result);
        }
    }
}