using System;
using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public class VehicleGraphType : ObjectGraphType<Vehicle> {
        public VehicleGraphType() {
            Name = "vehicle";
            Field(c => c.VehicleModel,
                nullable: false,
                typeof(ModelGraphType)).Description("What model of vehicle is this?");
            Field(c => c.Registration);
            Field(c => c.Color);
            Field(c => c.Year);
        }
    }
}
