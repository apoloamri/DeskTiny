using DeskTiny.Mvc.CustomAttributes;
using DeskTiny.Tools;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeskTiny.Mvc
{
    public class CustomController : Controller
    {
        public Model GetJson<Model>()
        {
            Type type = typeof(Model);

            object obj = Activator.CreateInstance(type);
            
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var request = reader.ReadToEndAsync();
                var jsonObject = (JObject)JsonConvert.DeserializeObject(request.Result);

                var dictionary = new Dictionary<string, object>();

                foreach (var token in jsonObject)
                {
                    dictionary.Add(token.Key, token.Value.ToObject<object>());
                }
                
                return DictionaryClassConverter.DictionaryToClass<Model>(dictionary);
            }
        }
        
        public override JsonResult Json(object data)
        {
            var jsonDictionary = new Dictionary<string, object>();
            var properties = data.GetType().GetProperties();
            
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                
                foreach (var attribute in attributes)
                {
                    var customAttribute = attribute as JsonResultAttribute;

                    if (customAttribute != null)
                    {
                        jsonDictionary.Add(property.Name, property.GetValue(data));
                    }
                }
            }

            var jsonSettings = new JsonSerializerSettings();

            jsonSettings.Formatting = Formatting.Indented;
            
            return base.Json(jsonDictionary, jsonSettings);
        }
    }
}
