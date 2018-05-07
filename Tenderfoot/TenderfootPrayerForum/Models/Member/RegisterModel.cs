using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.Net;
using Tenderfoot.Tools;
using TenderfootPrayerForum.Database;

namespace TenderfootPrayerForum.Models.Member
{
    public class RegisterModel : TfModel<Register>
    {
        [Input]
        [RequireInput]
        public Members Member { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            var fieldNames = new[] 
            {
                nameof(this.Member.email),
                nameof(this.Member.password),
                nameof(this.Member.first_name),
                nameof(this.Member.last_name),
                nameof(this.Member.birthdate),
                nameof(this.Member.gender)
            };
            
            if (this.IsValid(fieldNames))
            {
                yield return this.Library.ConfirmEmail(this.Member.email, nameof(this.Member.email));
            }
        }
        
        public override void HandleModel()
        {
            do
            {
                var uniqueKey = KeyGenerator.GetUniqueKey(64);
                this.Library.Members.ClearCase();
                this.Library.Members.Entity.activation_key = uniqueKey;
                this.Library.Members.Entity.active = 0;
            }
            while (this.Library.Members.Count > 0);
            this.Library.Members.Entity.SetValuesFromModel(this.Member, false);
            this.Library.Members.Insert();
            TfEmail.Send(this.Member.email, "member_register", this.Library.Members.Entity.ToDictionary());
        }
    }
}