using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;
using TenderfootPrayerForum.Database;

namespace TenderfootPrayerForum.Models.Member
{
    public class MemberModel : TfModel
    {
        [JsonProperty]
        public Members Member { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.ValidateSession();
        }

        public override void MapModel()
        {
            var members = _DB.Members;
            members.Entity.email = Encryption.Decrypt(this.SessionId);
            members.Entity.active = 1;
            this.Member = members.Select.Entity;
            this.Member.password = null;
        }
    }
}