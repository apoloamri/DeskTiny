using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Tenderfoot.Mvc.System;
using Tenderfoot.TfSystem;
using Tenderfoot.TfSystem.Diagnostics;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Mvc
{
    public class TfController : BaseController
    {
        [HttpGet]
        [Route("su")]
        public JsonResult GetAuthorization()
        {
            if (Settings.System.Debug)
            {
                this.Initiate<AuthorizeModel>(false);
                return this.Conclude();
            }

            this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return null;
        }

        public void Initiate<Model>(bool authorize = true) where Model : TfModel, new()
        {
            try
            {
                var obj = Activator.CreateInstance(typeof(Model));

                if (!this.Authorize(authorize))
                {
                    this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
                
                this.GetBody(obj);
                this.GetQueries(obj);
                this.ModelObject = DictionaryClassConverter.DictionaryToClass<Model>(this.ModelDictionary);
                this.GetNecessities();
                this.ModelObject.BeforeStartUp();
                this.ValidateModel();
                this.ModelObject.OnStartUp();
            }
            catch (Exception ex) when (!Settings.System.Debug)
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                TfDebug.WriteLog(ex);
            }
        }

        private void ValidateModel()
        {
            var validationDictionary = new Dictionary<string, object>();
            
            foreach (var property in this.ModelObject.GetType().GetProperties())
            {
                foreach (var attribute in property.GetCustomAttributes(false))
                {
                    if (attribute is ValidateInputAttribute)
                    {
                        var value = property.GetValue(this.ModelObject);

                        if (value != null)
                        {
                            var validateInputAttribute = attribute as ValidateInputAttribute;
                            var result = TfValidationResult.ValidateInput(validateInputAttribute.InputType, value, property.Name);
                            this.AddModelErrors(property.Name, result, ref validationDictionary);
                        }
                    }
                }
            }

            if (this.ModelObject.Validate() is IEnumerable<ValidationResult> validationResults)
            {
                foreach (var result in validationResults)
                {
                    if (result == null)
                    {
                        continue;
                    }

                    foreach (var name in result.MemberNames)
                    {
                        this.AddModelErrors(name, result, ref validationDictionary);
                    }
                }
            }

            var jsonDictionary = new Dictionary<string, object>();

            if (validationDictionary.Count() > 0)
            {
                jsonDictionary.Add("is_valid", false);
                jsonDictionary.Add("messages", validationDictionary);
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            this.JsonResult = base.Json(jsonDictionary, this.JsonSettings);
        }

        private void ExecuteMapping()
        {
            if (this.ModelObject.Mapping)
            {
                this.ModelObject.MapModel();
            }
        }

        private void ExecuteHandling()
        {
            if (this.ModelObject.Handling)
            {
                this.ModelObject.HandleModel();
            }
        }

        private void BuildModelDictionary()
        {
            var jsonDictionary = new Dictionary<string, object>();
            var properties = this.ModelObject.GetType().GetProperties();
            
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                
                foreach (var attribute in attributes)
                {
                    if (attribute is JsonPropertyAttribute)
                    {
                        var value = property.GetValue(this.ModelObject);

                        if (value != null)
                        {
                            jsonDictionary.Add(StringExtensions.ToUnderscore(property.Name), value);
                        }
                    }
                }
            }

            jsonDictionary.Add("is_valid", true);

            this.JsonResult = base.Json(jsonDictionary, this.JsonSettings);
        }

        public JsonResult Conclude()
        {
            try
            {
                if (this.ModelObject != null && 
                    this.ModelState.IsValid)
                {
                    this.ExecuteMapping();
                    this.ExecuteHandling();
                    this.BuildModelDictionary();
                    this.Response.StatusCode = (int)HttpStatusCode.OK;
                }
            }
            catch (Exception ex) when (!Settings.System.Debug)
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                TfDebug.WriteLog(ex);
            }
            
            return this.JsonResult;
        }

        public override ViewResult View()
        {
            return this.View(this.Conclude());
        }
    }
}
