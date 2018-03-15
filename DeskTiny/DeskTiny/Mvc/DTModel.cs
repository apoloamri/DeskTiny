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
        public HttpMethod HttpMethod { get; set; }
    }
}
