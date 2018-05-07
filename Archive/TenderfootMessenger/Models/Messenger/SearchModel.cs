using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TenderfootMessenger.Models.Messenger
{
    public class SearchModel : TfModel
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
                members.Conditions.Where(members.Column(x => x.username), Is.EqualTo, $"{this.Username}");
            }
            
            if (!this.Email.IsEmpty())
            {
                members.Conditions.Where(Operator.OR, members.Column(x => x.email), Is.EqualTo, $"{this.Email}");
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
