using DeskTiny.Mvc.CustomAttributes;
using DeskTiny.Mvc.System;
using DeskTiny.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeskTiny.Mvc
{
    public class CustomController : BaseController
    {
        public void BindModel<Model>(ref Model model)
        {
            var dictionary = new Dictionary<string, object>();

            Type type = typeof(Model);
            object obj = Activator.CreateInstance(type);
            
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var request = reader.ReadToEndAsync();
                var jsonObject = (JObject)JsonConvert.DeserializeObject(request.Result);
                
                if (jsonObject != null)
                {
                    foreach (var token in jsonObject)
                    {
                        var property = obj.GetType().GetProperty(token.Key);

                        foreach (var attribute in property.GetCustomAttributes(false))
                        {
                            if (attribute is InputAttribute)
                            {
                                dictionary.Add(token.Key, token.Value.ToObject<object>());
                            }
                        }
                    }
                }
            }

            foreach (var property in model.GetType().GetProperties())
            {
                if (!dictionary.ContainsKey(property.Name))
                {
                    foreach (var attribute in property.GetCustomAttributes(false))
                    {
                        if (attribute is InputAttribute)
                        {
                            dictionary.Add(property.Name, property.GetValue(model));
                        }
                    }
                }
            }

            model = DictionaryClassConverter.DictionaryToClass<Model>(dictionary);
        }
        
        public void BuildJson(object data)
        {
            if (!this.ModelState.IsValid)
            {
                return;
            }

            var jsonDictionary = new Dictionary<string, object>();
            var properties = data.GetType().GetProperties();
            
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                
                foreach (var attribute in attributes)
                {
                    if (attribute is JsonResultAttribute)
                    {
                        var value = property.GetValue(data);

                        if (value != null)
                        {
                            jsonDictionary.Add(property.Name, value);
                        }
                    }
                }
            }
            
            this.JsonResult = base.Json(jsonDictionary, this.jsonSettings);
        }

        public void Validate()
        {
            if (!this.ModelState.IsValid)
            {
                this.JsonResult = base.Json("problem occured", jsonSettings);
            }
        }
    }
}
