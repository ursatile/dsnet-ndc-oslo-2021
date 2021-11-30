using System.Collections.Generic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Queries {
    public class VehicleQuery : ObjectGraphType {
        private readonly IAutobarnDatabase db;

        public VehicleQuery(IAutobarnDatabase db) {
            this.db = db;
            Field<ListGraphType<VehicleGraphType>>("Vehicles",
                "Query to return every vehicle in the system",
                resolve: GetAllVehicles);

            Field<VehicleGraphType>("Vehicle",
                "Query to return a single vehicle",
                new QueryArguments(
                    MakeNonNullStringArgument("registration", "The registration plate of the car you want")),

                resolve: GetVehicle);

            Field<ListGraphType<VehicleGraphType>>("VehiclesByColor",
                "Retrieve all vehicles of a specified color",

                new QueryArguments(
                    MakeNonNullStringArgument("color", "What color vehicles do you want?")),
                resolve: GetVehiclesByColor);


        }

        private QueryArgument MakeNonNullStringArgument(string name, string description) {
            return new QueryArgument<NonNullGraphType<StringGraphType>> {
                Name = name, Description = description
            };
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> context) => db.ListVehicles();

        private Vehicle GetVehicle(IResolveFieldContext<object> context) {
            var registration = context.GetArgument<string>("registration");
            return db.FindVehicle(registration);
        }

        private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext<object> context) {
            var color = context.GetArgument<string>("color");
            if (color == "*") return db.ListVehicles();
            return db.ListVehicles().Where(v => v.Color == color);
        }
    }
}