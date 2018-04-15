using DTCore.Database;
using DTCore.Mvc;
using DTCore.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTMessenger.Models.Messenger
{
    public class SearchModel : DTModel
    {
        [Input]
        public string Email { get; set; }

        [Input]
        public string Username { get; set; }

        [JsonProperty]
        public List<Dictionary<string, object>> Result { get; set; }

        public override void HandleModel()
        {
            throw new NotImplementedException();
        }

        public override void MapModel()
        {
            if (this.Email.IsEmpty() && this.Username.IsEmpty())
            {
                return;
            }

            var members = DT.Database.Schemas.Members;

            if (!this.Username.IsEmpty())
            {
                members.Conditions.Where(members.Column(x => x.username), Condition.EqualTo, $"{this.Username}");
            }
            
            if (!this.Email.IsEmpty())
            {
                members.Conditions.Where(Operator.OR, members.Column(x => x.email), Condition.EqualTo, $"{this.Email}");
            }
            
            this.Result = members.Select.Dictionaries
                .Where(x => (string)x["username"] != this.SessionId)
                .ToList();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }
    }
}
