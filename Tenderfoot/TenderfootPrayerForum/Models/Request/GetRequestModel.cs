using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;
using TenderfootPrayerForum.Database;

namespace TenderfootPrayerForum.Models.Request
{
    public class GetRequestModel : TfModel<GetRequest>
    {
        [Input]
        [RequireInput]
        public int? Id { get; set; }

        public int Active { get { return 1; } }
        
        [JsonProperty]
        public Requests Request { get; set; }

        [JsonProperty]
        public bool IsRepliable { get; set; } = false;
        
        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.ValidateSession();
            if (this.IsValid(
                nameof(this.Id),
                nameof(this.SessionId),
                nameof(this.SessionKey)))
            {
                this.Library.Requests.Entity.SetValuesFromModel(this);
                this.Library.Members.Entity.email = Encryption.Decrypt(this.SessionId);
                this.Library.ValidateRequest();
                this.Library.ValidateMember();
            }
        }

        public override void MapModel()
        {
            this.Request = this.Library.Requests.Select.Entity;
            this.IsRepliable = this.Library.IsRepliable;
        }
    }
}