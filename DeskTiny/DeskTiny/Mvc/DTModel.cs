using DTCore.Mvc.Attributes;
using DTCore.Mvc.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTCore.Mvc
{
    public abstract class DTModel
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
    }
}
