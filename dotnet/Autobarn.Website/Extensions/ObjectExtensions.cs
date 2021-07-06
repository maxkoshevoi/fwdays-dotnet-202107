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
                if (Ignore(prop))
                {
                    continue;
                }

                expando.Add(prop.Name.ToLower(), prop.GetValue(value));
            }
            return (ExpandoObject)expando;

            static bool Ignore(PropertyDescriptor prop) => prop.Attributes.OfType<JsonIgnoreAttribute>().Any();
        }

        public static dynamic ToResource(this Vehicle vehicle)
        {
            var resource = vehicle.ToDynamic();
            resource._links = new
            {
                self = new { href = $"/api/vehicles/{vehicle.Registration}" },
                viewModel = new { href = $"/api/models/{vehicle.ModelCode}" },
            };
            return resource;
        }
    }
}
