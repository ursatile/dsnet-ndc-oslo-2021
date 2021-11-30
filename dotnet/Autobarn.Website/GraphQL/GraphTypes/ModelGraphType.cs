using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public class ModelGraphType : ObjectGraphType<Model> {
        public ModelGraphType() {
            Name = "VehicleModel";
            Field(m => m.Name).Description("The name of this vehicle model");
            Field(m => m.Manufacturer,
                    nullable: false, typeof(ManufacturerGraphType))
                .Description("Which company manufactures this model?");
        }
    }
}