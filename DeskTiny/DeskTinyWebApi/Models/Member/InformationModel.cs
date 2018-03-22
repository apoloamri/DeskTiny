using DeskTinyWebApi.DT.Database;
using DTCore.Database.Enums;
using DTCore.Mvc;
using DTCore.Mvc.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeskTinyWebApi.Models.Member
{
    public class InformationModel : DTModel
    {
        [Input]
        public string Username { get; set; }

        [JsonProperty]
        public Dictionary<string, object> MemberDictionary { get; set; }

        public override void HandleModel()
        {
        }

        public override void MapModel()
        {
            var member = Schemas.Members;

            member.Conditions.Where(
                member.Column(x => x.username),
                Condition.EqualTo,
                this.Username);
            
            this.MemberDictionary = member.Select.Dictionary;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }
    }
}
