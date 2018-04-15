using DTCore.Database;
using DTCore.Mvc;
using DTTemplateAPI.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTTemplateAPI.Models.Home
{
    public class IndexModel : DTModel
    {
        [Input]
        public string Username { get; set; }

        public override void HandleModel()
        {
            
        }

        public override void MapModel()
        {
            var members = _DB.Members;
            members.Conditions.Where(members.Column(x => x.username), Condition.EqualTo, this.Username);
            var result = members.Select.Entities;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Mapping)
            {
                yield return DTValidationResult.FieldRequired(nameof(this.Username), this.Username);
            }
        }
    }
}
