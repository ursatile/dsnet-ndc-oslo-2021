using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Autobarn.Data.Entities;
using Newtonsoft.Json;

namespace Autobarn.Website.Controllers.api {
    public static class HypermediaExtensions {

        public static dynamic ToResource(this Vehicle vehicle) {
            var resource = vehicle.ToDynamic();
            resource._links = new {
                self = new {
                    href = $"/api/vehicles/{vehicle.Registration}",
                },
                model = new {
                    href = $"/api/models/{vehicle.ModelCode}"
                }
            };
            return resource;
        }

        public static dynamic ToDynamic(this object value) {
            IDictionary<string, object> expando = new ExpandoObject();
            var properties = TypeDescriptor.GetProperties(value.GetType());
            foreach (PropertyDescriptor prop in properties) {
                if (Ignore(prop)) continue;
                expando.Add(prop.Name, prop.GetValue(value));
            }
            return (ExpandoObject)expando;
        }

        private static bool Ignore(PropertyDescriptor prop) {
            return prop.Attributes.OfType<JsonIgnoreAttribute>().Any();
        }

        public static dynamic Paginate(string url, int index, int count, int total) {
            dynamic links = new ExpandoObject();
            links.self = new { href = $"{url}?index={index}&count={count}" };
            links.final = new { href = $"{url}?index={total - (total % count)}&count={count}" };
            links.first = new { href = $"{url}?index=0&count={count}" };
            if (index > 0) links.previous = new { href = $"{url}?index={index - count}&count={count}" };
            if (index + count < total) links.next = new { href = $"{url}?index={index + count}&count={count}" };
            return links;
        }

    }
}