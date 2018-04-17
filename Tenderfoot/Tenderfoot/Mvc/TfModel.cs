using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Mvc
{
    public abstract class TfModel<T> : TfModel where T : class, new()
    {
        public readonly T Library = new T();
    }

    public abstract class TfModel
    {
        public abstract void HandleModel();
        public abstract void MapModel();
        public abstract IEnumerable<ValidationResult> Validate();

        public Method Method { get; set; }
        public bool Mapping => this.Method == Method.GET;
        public bool Handling => this.Method != Method.GET;

        [Input]
        [JsonProperty]
        public virtual string SessionKey { get; set; }

        [Input]
        [JsonProperty]
        public virtual string SessionId { get; set; }

        public virtual void BeforeStartUp() { }
        public virtual void OnStartUp() { }

        public bool IsValid(params string[] fieldNames)
        {
            var boolList = new List<bool>();

            foreach (var fieldName in fieldNames)
            {
                var field = this.GetType().GetProperty(fieldName)?.GetValue(this);

                if (field != null && !field.ToString().IsEmpty())
                {
                    boolList.Add(true);
                }
                else
                {
                    boolList.Add(false);
                }
            }

            if (boolList.All(x => x == true))
            {
                return true;
            }

            return false;
        }
    }
}
