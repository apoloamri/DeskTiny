using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Tenderfoot.Mvc.System;
using Tenderfoot.TfSystem.Diagnostics;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Mvc
{
    public class TfController : BaseController
    {
        public void Initiate<Model>(bool validate = false) where Model : TfModel, new()
        {
            try
            {
                var obj = Activator.CreateInstance(typeof(Model));

                this.GetMethod();
                this.GetBody(obj);
                this.GetQueries(obj);

                this.ModelObject = DictionaryClassConverter.DictionaryToClass<Model>(this.ModelDictionary);
                this.ModelObject.BeforeStartUp();

                if (validate)
                {
                    this.ValidateModel();
                }

                this.ModelObject.OnStartUp();
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                TfDebug.WriteLog(ex);
            }
        }

        private void ValidateModel()
        {
            var jsonDictionary = new Dictionary<string, object>();
            var validationDictionary = new Dictionary<string, object>();
            
            if (this.ModelObject.Validate() is IEnumerable<ValidationResult> validationResults)
            {
                var validationList = new List<Dictionary<string, string>>();

                foreach (var result in validationResults)
                {
                    if (result == null)
                    {
                        continue;
                    }

                    foreach (var name in result.MemberNames)
                    {
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
                }

                if (validationDictionary.Count() > 0)
                {
                    jsonDictionary.Add("is_valid", false);
                    jsonDictionary.Add("messages", validationDictionary);
                }
                
                this.JsonResult = base.Json(jsonDictionary, this.JsonSettings);
            }
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
                }
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
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
