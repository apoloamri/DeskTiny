﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
        protected Dictionary<string, object> Messages
        {
            get
            {
                var dictionary = JsonResult.Value as Dictionary<string, object>;
                return dictionary?["messages"] as Dictionary<string, object>;
            }
        }

        protected bool Authorize(bool validate)
        {
            if (!validate)
            {
                return true;
            }
            
            var accesses = Schemas.Accesses;

            if (TfSettings.System.Debug)
            {
                accesses.Case.Where(accesses._(x => x.key), Is.EqualTo, TfSettings.System.DefaultKey);
                accesses.Case.Where(accesses._(x => x.secret), Is.EqualTo, TfSettings.System.DefaultSecret);
                if (accesses.Count == 0)
                {
                    var uri = new Uri(TfSettings.Web.ApiUrl);
                    accesses.Entity.key = TfSettings.System.DefaultKey;
                    accesses.Entity.secret = TfSettings.System.DefaultSecret;
                    accesses.Entity.host = $"{uri.Host}:{uri.Port}";
                    accesses.Insert();
                }
            }

            accesses.ClearCase();
            accesses.Case.Where("CONCAT(key, secret, host) = {0}", Encryption.Decrypt(this.Request.Headers["Authorization"].ToString()));
            //accesses.Case.Where(accesses._(x => x.host), Is.EqualTo, this.Request.Host.ToString());
            accesses.Case.Where(accesses._(x => x.active), Is.EqualTo, 1);

            if (accesses.Count == 0 || (TfSettings.Web.RequireHttps && this.Request.Scheme != "https"))
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

        protected void GetNecessities()
        {
            Enum.TryParse(this.Request.Method, out Method httpMethod);
            this.ModelObject.Method = httpMethod;
            this.ModelObject.Controller = this;
        }

        protected void GetBody(object obj)
        {
            using (StreamReader reader = new StreamReader(this.Request.Body, Encoding.UTF8))
            {
                var request = reader.ReadToEndAsync();
                
                if (JsonTools.IsValidJson(request.Result))
                {
                    this.ModelDictionary = this.GetJson(request.Result, obj);
                }
                else
                {
                    var json = JsonTools.QueryStringToJson(request.Result);
                    this.ModelDictionary = this.GetJson(json, obj);
                }
            }
        }

        protected void GetQueries(object obj)
        {
            foreach (var query in this.Request.Query)
            {
                string propertyName = query.Key;

                var property = this.GetModelProperty(ref propertyName, obj);

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

        protected void AddModelErrors(string name, ValidationResult result, ref Dictionary<string, object> validationDictionary)
        {
            if (result == null)
            {
                return;
            }

            var keyName = name.ToUnderscore();

            if (validationDictionary.ContainsKey(keyName))
            {
                validationDictionary[keyName] += Environment.NewLine + result.ErrorMessage;
            }
            else
            {
                validationDictionary.Add(keyName, result.ErrorMessage);
            }

            this.ControllerContext.ModelState.AddModelError(name, result.ErrorMessage);
        }

        private PropertyInfo GetModelProperty(ref string name, object obj)
        {
            if (name == null)
            {
                return null;
            }

            name = name.Replace("[]", "");

            var type = obj.GetType();
            var property = type.GetProperty(name);

            if (property == null)
            {
                name = name.ToCamelCase();
                property = obj.GetType().GetProperty(name);

                if (property == null)
                {
                    foreach (var prop in type.GetProperties())
                    {
                        var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
                        if (attribute != null)
                        {
                            if (attribute.DisplayName == name)
                            {
                                return prop;
                            }

                            if (attribute.DisplayName == name.ToUnderscore())
                            {
                                return prop;
                            }
                        }
                    }
                }
            }

            return property;
        }

        private Dictionary<string, object> GetJson(string result, object obj)
        {
            var returnDictionary = new Dictionary<string, object>();
            var jsonObject = (JObject)JsonConvert.DeserializeObject(result);

            if (jsonObject != null)
            {
                foreach (var token in jsonObject)
                {
                    string propertyName = token.Key;

                    var property = this.GetModelProperty(ref propertyName, obj);
                    
                    if (property == null || this.ModelDictionary.ContainsKey(propertyName))
                    {
                        continue;
                    }

                    var type = property.PropertyType;

                    if (property.GetCustomAttribute<InputAttribute>(false) != null)
                    {
                        var jsonString = token.Value.ToString();
                        var isValidJson = JsonTools.IsValidJson(jsonString);

                        if (isValidJson && JToken.Parse(jsonString) is JArray)
                        {
                            var itemType = 
                                type.IsArray ? 
                                type.GetGenericArguments()[0] :
                                type;
                            
                            if (itemType.GetConstructor(Type.EmptyTypes) != null &&
                                !itemType.IsAbstract)
                            {
                                var objectClass = Activator.CreateInstance(itemType);
                                var list = new List<Dictionary<string, object>>();

                                foreach (var item in token.Value.ToObject<List<object>>())
                                {
                                    list.Add(this.GetJson(item.ToString(), objectClass));
                                }

                                if (type.IsArray)
                                {
                                    returnDictionary.Add(propertyName, list);
                                }
                                else
                                {
                                    returnDictionary.Add(propertyName, list.FirstOrDefault());
                                }
                            }
                            else
                            {
                                returnDictionary.Add(propertyName, token.Value.ToObject<List<dynamic>>());
                            }
                        }
                        else
                        {
                            if (type.GetConstructor(Type.EmptyTypes) != null &&
                                !type.IsAbstract)
                            {
                                returnDictionary.Add(propertyName, token.Value.ToObject<Dictionary<string, object>>());
                            }
                            else
                            {
                                var tokenObject = token.Value.ToObject<object>();
                                returnDictionary.Add(propertyName, tokenObject);
                            }
                        }
                    }
                }
            }

            return returnDictionary;
        }
    }
}
