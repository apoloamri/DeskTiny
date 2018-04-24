using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;
using TenderfootCampaign.Library.Home;

namespace TenderfootCampaign.Models.Home
{
    public class LoginModel : TfModel<Login>
    {
        public override void BeforeStartUp()
        {
            this.Library.PopulateMembers(this.Username, this.Password);
        }

        [Input]
        [ValidateInput(InputType.String, 50)]
        public string Username { get; set; }

        [Input]
        [ValidateInput(InputType.All, 100)]
        public string Password { get; set; }

        public override void HandleModel()
        {
            this.SessionId = Encryption.Encrypt(this.Username);
            this.SessionKey = Session.AddSession(this.Username);
        }

        public override void MapModel()
        {
            this.SessionId = Encryption.Encrypt(this.Username);
            this.SessionKey = this.SessionKey;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Mapping)
            {
                yield return TfValidationResult.FieldRequired(nameof(this.SessionId), this.SessionId);
                yield return TfValidationResult.FieldRequired(nameof(this.SessionKey), this.SessionKey);
                if (this.IsValid(nameof(this.SessionId), nameof(this.SessionKey)))
                {
                    yield return TfValidationResult.CheckSessionActivity(this.SessionId, this.SessionKey);
                }
            }

            if (this.Handling)
            {
                yield return TfValidationResult.FieldRequired(nameof(this.Username), this.Username);
                yield return TfValidationResult.FieldRequired(nameof(this.Password), this.Password);
                if (this.IsValid(nameof(this.Username), nameof(this.Password)))
                {
                    yield return this.Library.CheckUsernamePassword(nameof(this.Username), nameof(this.Password));
                }
            }
        }
    }
}
