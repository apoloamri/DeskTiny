using DTCore.Mvc.Attributes;
using DTCore.Mvc.System;
using DTCore.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DTCore.Mvc
{
    public class DTController : BaseController
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
                            if (attribute is Input)
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
                        if (attribute is Input)
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
                    if (attribute is JsonResult)
                    {
                        var value = property.GetValue(data);

                        if (value != null)
                        {
                            string propertyName = 
                                string.Concat(property.Name.Select((x, i) => 
                                    i > 0 && char.IsUpper(x) ? 
                                    "_" + x.ToString()?.ToLower() : 
                                    x.ToString().ToLower()));

                            jsonDictionary.Add(propertyName, value);
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
