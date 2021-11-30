using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public class ManufacturerGraphType : ObjectGraphType<Manufacturer> {

        public ManufacturerGraphType() {
            Name = "Manufacturer";
            Field(c => c.Name).Description("The name of the manufacturer");
        }
    }
}