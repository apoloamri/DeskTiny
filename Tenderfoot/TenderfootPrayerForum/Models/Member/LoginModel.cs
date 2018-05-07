using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace TenderfootPrayerForum.Models.Member
{
    public class LoginModel : TfModel<Login>
    {
        [Input]
        [ValidateInput(InputType.Email)]
        public string Email { get; set; }

        [Input]
        [ValidateInput(InputType.All)]
        public string Password { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Mapping)
            {
                yield return this.ValidateSession();
            }

            if (this.Handling)
            {
                yield return this.FieldRequired(nameof(this.Email));
                yield return this.FieldRequired(nameof(this.Password));
                if (this.IsValid(nameof(this.Email), nameof(this.Password)))
                {
                    this.Library.Members.Entity.SetValuesFromModel(this);
                    yield return this.Library.CheckMember(nameof(this.Email), nameof(this.Password));
                    yield return this.Library.CheckActivity(nameof(this.Email));
                }
            }
        }

        public override void HandleModel()
        {
            this.NewSession(this.Email);
        }
    }
}