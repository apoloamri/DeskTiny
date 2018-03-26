using DTMessenger.DT.Database;
using DTCore.Database.Enums;
using DTCore.Mvc;
using DTCore.Mvc.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTMessenger.Models.Member
{
    public class InformationModel : DTModel
    {
        [JsonProperty]
        public Dictionary<string, object> Result { get; set; }

        public override void HandleModel()
        {
        }

        public override void MapModel()
        {
            var member = Schemas.Members;

            member.Conditions.Where(
                member.Column(x => x.username),
                Condition.EqualTo,
                this.SessionId);
            
            this.Result = member.Select.Dictionary;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return DTValidationResult.CheckSessionActivity(this.SessionId, this.SessionKey);
        }
    }
}
