using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TenderfootPrayerForum.Models.Home
{
    public class IndexModel : TfModel
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
                yield return TfValidationResult.FieldRequired(nameof(this.Username), this.Username);
            }
        }
    }
}
