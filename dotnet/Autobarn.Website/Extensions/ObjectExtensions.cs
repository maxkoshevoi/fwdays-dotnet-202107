using Autobarn.Data.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace Autobarn.Website.Extensions
{
    public static class ObjectExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            var properties = TypeDescriptor.GetProperties(value.GetType());
            foreach (PropertyDescriptor prop in properties)
            {
                expando.Add(prop.Name, prop.GetValue(value));
            }
            return (ExpandoObject)expando;
        }

        public static dynamic ToResource(this Vehicle vehicle)
        {
            var resource = vehicle.ToDynamic();
            
            // Ignore stuff
            ((IDictionary<string, object>)resource).Remove(nameof(Vehicle.VehicleModel));

            // Enrich
            resource._links = new
            {
                self = new { href = $"/api/vehicles/{vehicle.Registration}" },
                viewModel = new { href = $"/api/models/{vehicle.ModelCode}" },
            };
            return resource;
        }
    }
}
