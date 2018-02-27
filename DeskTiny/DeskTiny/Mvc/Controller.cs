using DeskTiny.Mvc.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DeskTiny.Mvc
{
    public class CustomController : Controller
    {
        public override JsonResult Json(object data)
        {
            var jsonDictionary = new Dictionary<string, object>();
            var properties = data.GetType().GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);

                foreach (var attribute in attributes)
                {
                    var customAttribute = attribute as ShowAtJsonResultAttribute;

                    if (customAttribute != null)
                    {
                        jsonDictionary.Add(property.Name, property.GetValue(data));
                    }
                }
            }

            return base.Json(jsonDictionary);
        }
    }
}
