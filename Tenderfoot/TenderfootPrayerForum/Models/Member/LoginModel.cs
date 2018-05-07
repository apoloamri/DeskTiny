using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace TenderfootPrayerForum.Models.Member
{
    public class LoginModel : TfModel<Login>
    {
        [Input]
        [ValidateInput(InputType.String, 50)]
        public string Username { get; set; }

        [Input]
        [ValidateInput(InputType.All, 100)]
        public string Password { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Mapping)
            {
                yield return this.ValidateSession();
            }

            if (this.Handling)
            {
                yield return this.FieldRequired(nameof(this.Username));
                yield return this.FieldRequired(nameof(this.Password));
                if (this.IsValid(nameof(this.Username), nameof(this.Password)))
                {
                    this.Library.Members.Entity.SetValuesFromModel(this);
                    yield return this.Library.CheckUsernamePassword(nameof(this.Username), nameof(this.Password));
                    yield return this.Library.CheckActivity(nameof(this.Username));
                }
            }
        }

        public override void HandleModel()
        {
            this.NewSession(this.Username);
        }

        public override void MapModel() { }
    }
}