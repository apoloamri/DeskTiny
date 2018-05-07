using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Mvc
{
    public abstract class TfModel<T> : TfModel where T : class, new()
    {
        public readonly T Library = new T();
    }

    public abstract class TfModel
    {
        public abstract IEnumerable<ValidationResult> Validate();

        public string Host { get; set; }
        public Controller Controller { get; set; }
        public Method Method { get; set; }
        public bool Mapping => this.Method == Method.GET;
        public bool Handling => this.Method != Method.GET;
        public bool Stop { get; private set; }
        public List<string> InvalidFields { get; set; } = new List<string>();
        
        [Input]
        [JsonProperty]
        [ValidateInput(InputType.All, 100)]
        public virtual string SessionKey { get; set; }

        [Input]
        [JsonProperty]
        [ValidateInput(InputType.All, 100)]
        public virtual string SessionId { get; set; }

        public virtual void BeforeStartUp() { }
        public virtual void OnStartUp() { }
        public virtual void MapModel() { throw new NotImplementedException(); }
        public virtual void HandleModel() { throw new NotImplementedException(); }

        public bool IsValid(params string[] fieldNames)
        {
            foreach (var fieldName in fieldNames)
            {
                if (this.InvalidFields.Contains(fieldName))
                {
                    return false;
                }
            }
            return true;
        }

        public ValidationResult FieldRequired(string fieldName)
        {
            var validation = TfValidationResult.FieldRequired(fieldName, this.GetType().GetProperty(fieldName)?.GetValue(this));
            if (validation != null)
            {
                this.InvalidFields.Add(fieldName);
            }
            return validation;
        }

        public ValidationResult ValidateSession()
        {
            var validation = TfValidationResult.CheckSessionActivity(
                this.SessionId, 
                this.SessionKey, 
                nameof(this.SessionId),
                nameof(this.SessionKey));

            if (validation != null)
            {
                this.InvalidFields.Add(nameof(this.SessionId));
                this.InvalidFields.Add(nameof(this.SessionKey));
            }

            return validation;
        }

        public bool SessionActive()
        {
            return this.ValidateSession() == null;
        }

        public void NewSession(string sessionId)
        {
            this.SessionId = Session.AddSession(sessionId, out string sessionKey);
            this.SessionKey = sessionKey;
        }

        public void SetValuesFromModel(object model)
        {
            var type = model.GetType();

            foreach (var property in type.GetProperties())
            {
                var thisProperty = this.GetType().GetProperty(property.Name.ToUnderscore());
                if (this.HasAttribute(thisProperty))
                {
                    thisProperty.SetValue(this, property.GetValue(model));
                }
            }
        }

        public void SetValuesFromDictionary(Dictionary<string, object> dictionary)
        {
            foreach (var item in dictionary)
            {
                var thisProperty = this.GetType().GetProperty(item.Key.ToUnderscore());
                if (this.HasAttribute(thisProperty))
                {
                    thisProperty.SetValue(this, item.Value);
                }
            }
        }

        public void StopProcess()
        {
            this.Stop = true;
        }

        private bool HasAttribute(PropertyInfo property)
        {
            return
                property != null && 
                (property?.GetCustomAttribute<InputAttribute>(false) != null ||
                property?.GetCustomAttribute<JsonPropertyAttribute>(false) != null);
        }
    }
}
