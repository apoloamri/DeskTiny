using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tenderfoot.Mvc
{
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
    }
}
