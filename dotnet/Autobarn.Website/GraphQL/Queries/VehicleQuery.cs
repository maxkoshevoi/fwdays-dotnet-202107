using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;

namespace Autobarn.Website.GraphQL.Queries
{
	public class VehicleQuery : ObjectGraphType
	{
		private readonly IAutobarnDatabase db;

		public VehicleQuery(IAutobarnDatabase db)
		{
			this.db = db;

			Field<ListGraphType<VehicleGraphType>>("Vehicles", "Query to retrieve all Vehicles",
				resolve: GetAllVehicles);
		}

		private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> context) => db.ListVehicles();
	}
}
