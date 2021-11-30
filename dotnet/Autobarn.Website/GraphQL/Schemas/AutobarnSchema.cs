using System.Collections.Generic;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using Autobarn.Website.GraphQL.Queries;
using GraphQL;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Schemas {
    public class AutobarnSchema : Schema {
        public AutobarnSchema(IAutobarnDatabase db) => Query = new VehicleQuery(db);

    }
}

namespace Autobarn.Website.GraphQL.Queries {
    public class VehicleQuery : ObjectGraphType {
        private readonly IAutobarnDatabase db;

        public VehicleQuery(IAutobarnDatabase db) {
            this.db = db;
            Field<ListGraphType<VehicleGraphType>>("Vehicles",
                "Query to return every vehicle in the system",
                resolve: GetAllVehicles);
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> context) => db.ListVehicles();
    }
}
