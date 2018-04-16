using Tenderfoot.Database;
using Tenderfoot.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TenderfootMessenger.Models.Member
{
    public class InformationModel : TfModel
    {
        [JsonProperty]
        public Dictionary<string, object> Result { get; set; }

        public override void HandleModel()
        {
        }

        public override void MapModel()
        {
            var member = DT.Database.Schemas.Members;

            member.Conditions.Where(
                member.Column(x => x.username),
                Condition.EqualTo,
                this.SessionId);
            
            this.Result = member.Select.Dictionary;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return TfValidationResult.CheckSessionActivity(this.SessionId, this.SessionKey);
        }
    }
}
