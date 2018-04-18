using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Tenderfoot.Database;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Mvc.System
{
    public class BaseController : Controller
    {
        protected dynamic ModelObject { get; set; }
        protected JsonSerializerSettings JsonSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
        protected JsonResult JsonResult { get; set; }
        protected Dictionary<string, object> ModelDictionary { get; set; } = new Dictionary<string, object>();

        protected PropertyInfo GetModelProperty(ref string name, object obj)
        {
            if (name == null)
            {
                return null;
            }

            name = name.Replace("[]", "");

            var property = obj.GetType().GetProperty(name);

            if (property == null)
            {
                name = name.ToCamelCase();
                property = obj.GetType().GetProperty(name);

                if (property == null)
                {
                    return null;
                }
            }

            return property;
        }

        protected bool Authorize(bool validate)
        {
            if (!validate)
            {
                return true;
            }

            var accesses = Schemas.Accesses;

            if (Settings.System.Debug)
            {
                accesses.Conditions.Where(accesses.Column(x => x.key), Is.EqualTo, Settings.System.DefaultKey);
                accesses.Conditions.Where(accesses.Column(x => x.secret), Is.EqualTo, Settings.System.DefaultSecret);
                if (accesses.Count() == 0)
                {
                    accesses.Entity.key = Settings.System.DefaultKey;
                    accesses.Entity.secret = Settings.System.DefaultSecret;
                    accesses.Insert();
                }
            }

            accesses.ClearConditions();
            accesses.Conditions.Where("CONCAT(key, secret) = {0}", Encryption.Decrypt(this.Request.Headers["Authorization"].ToString()));
            accesses.Conditions.Where(accesses.Column(x => x.active), Is.EqualTo, 1);

            if (accesses.Count() == 0)
            {
                var validationDictionary = new Dictionary<string, object>();
                var validationError = TfValidationResult.Compose("Unauthorized", "system");

                this.ControllerContext.ModelState.AddModelError("system", validationError.ErrorMessage);
                
                var jsonDictionary = new Dictionary<string, object>
                {
                    { "is_valid", false },
                    { "message", new Dictionary<string, object>{ { validationError.MemberNames.First(), validationError.ErrorMessage } } }
                };
                
                this.JsonResult = base.Json(jsonDictionary, this.JsonSettings);

                return false;
            }

            return true;
        }

        protected void GetMethod()
        {
            Enum.TryParse(this.Request.Method, out Method httpMethod);
            this.ModelDictionary.Add("Method", httpMethod);
        }

        protected void GetBody(object obj)
        {
            using (StreamReader reader = new StreamReader(this.Request.Body, Encoding.UTF8))
            {
                var request = reader.ReadToEndAsync();
                
                if (JsonTools.IsValidJson(request.Result))
                {
                    var jsonObject = (JObject)JsonConvert.DeserializeObject(request.Result);

                    if (jsonObject != null)
                    {
                        foreach (var token in jsonObject)
                        {
                            string propertyName = token.Key;

                            var property = GetModelProperty(ref propertyName, obj);

                            if (property == null || this.ModelDictionary.ContainsKey(propertyName))
                            {
                                continue;
                            }

                            if (property.GetCustomAttribute<InputAttribute>(false) != null)
                            {
                                this.ModelDictionary.Add(propertyName, token.Value.ToObject<object>());
                            }
                        }
                    }
                }
                else
                {
                    var body = HttpUtility.ParseQueryString(request.Result);

                    foreach (var item in body.AllKeys)
                    {
                        string propertyName = item;

                        var property = GetModelProperty(ref propertyName, obj);

                        if (property == null || this.ModelDictionary.ContainsKey(propertyName))
                        {
                            continue;
                        }

                        if (property.GetCustomAttribute<InputAttribute>(false) != null)
                        {
                            this.ModelDictionary.Add(propertyName, body[item]);
                        }
                    }
                }
            }
        }

        protected void GetQueries(object obj)
        {
            foreach (var query in this.Request.Query)
            {
                string propertyName = query.Key;

                var property = GetModelProperty(ref propertyName, obj);

                if (property == null || this.ModelDictionary.ContainsKey(propertyName))
                {
                    continue;
                }

                if (property.GetCustomAttribute<InputAttribute>(false) != null)
                {
                    this.ModelDictionary.Add(propertyName, query.Value.ToString());
                }
            }
        }
    }
}
