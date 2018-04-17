using Tenderfoot.Mvc;
using TenderfootMessenger.DT.Database;
using TenderfootMessenger.DT.Messenger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TenderfootMessenger.Models.Messenger
{
    public class AddModel : TfModel
    {
        [Input]
        public string Username { get; set; }
        
        public override void HandleModel()
        {
            var contacts = Schemas.Contacts;
            contacts.Entity.username = this.SessionId;
            contacts.Entity.contact_username = this.Username;
            contacts.Insert();
        }

        public override void MapModel()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return TfValidationResult.CheckSessionActivity(this.SessionId, this.SessionKey);
            yield return TfValidationResult.FieldRequired(nameof(this.Username), this.Username);

            if (CheckAdd.CheckContactExists(this.SessionId, this.Username))
            {
                yield return TfValidationResult.Compose("ContactExists", nameof(this.Username));
            }
        }
    }
}
