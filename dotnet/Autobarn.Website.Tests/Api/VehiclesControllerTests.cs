using System.Collections.Generic;
using System.Dynamic;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Controllers.api;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace Autobarn.Website.Tests.Api {
    public class VehiclesControllerTests {
        [Fact]
        public void GET_Vehicle_By_Id_Returns_Vehicle() {
            var c = new VehiclesController(
                new FakeDatabase(), null, null);
            var result = c.Get("TEST1234") as OkObjectResult;
            result.ShouldNotBeNull();
            result.Value.ShouldBeOfType<ExpandoObject>();
            var links = ((dynamic) result.Value)._links;
            var self = ((dynamic)links).self;
            ((dynamic)self).href.ShouldBe("/api/vehicles/TEST1234");
        }
    }
}

public class FakeDatabase : IAutobarnDatabase {
    public int CountVehicles() {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Vehicle> ListVehicles() {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Manufacturer> ListManufacturers() {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Model> ListModels() {
        throw new System.NotImplementedException();
    }

    public Vehicle FindVehicle(string registration) {
        return new Vehicle() {
            Registration = registration,
            Color = "blue",
            Year = 1987,
            VehicleModel = new Model {
                Name = "TestModel",
                Code = "test",
                Manufacturer = new Manufacturer {
                    Name = "Test",
                    Code = "test"
                }
            }
        };
    }

    public Model FindModel(string code) {
        throw new System.NotImplementedException();
    }

    public Manufacturer FindManufacturer(string code) {
        throw new System.NotImplementedException();
    }

    public void CreateVehicle(Vehicle vehicle) {
        throw new System.NotImplementedException();
    }

    public void UpdateVehicle(Vehicle vehicle) {
        throw new System.NotImplementedException();
    }

    public void DeleteVehicle(Vehicle vehicle) {
        throw new System.NotImplementedException();
    }
}

