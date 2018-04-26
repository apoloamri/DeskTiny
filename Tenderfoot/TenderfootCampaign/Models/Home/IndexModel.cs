using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace TenderfootCampaign.Models.Home
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
            members.Case.Where(members._(x => x.username), Is.EqualTo, this.Username);
            this.Result = members.Select.Dictionary;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.FieldRequired(nameof(this.Username));
        }
    }
}
