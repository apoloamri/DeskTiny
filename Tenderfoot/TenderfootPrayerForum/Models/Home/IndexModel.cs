using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Library._Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace TenderfootPrayerForum.Models.Home
{
    public class IndexModel : TfModel
    {
        [Input]
        public string Username { get; set; }

        [JsonProperty]
        public Dictionary<string, object> Result { get; set; }

        public override void HandleModel()
        {
            throw new NotImplementedException();
        }

        public override void MapModel()
        {
            var members = _DB.Members;
            members.Conditions.Where(members._(x => x.username), Is.EqualTo, this.Username);
            this.Result = members.Select.Dictionary;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return TfValidationResult.FieldRequired(nameof(this.Username), this.Username);
        }
    }
}
