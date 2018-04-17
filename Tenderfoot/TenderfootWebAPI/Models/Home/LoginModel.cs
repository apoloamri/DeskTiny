using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootWebAPI.Library.Home;

namespace TenderfootWebAPI.Models.Home
{
    public class LoginModel : TfModel<Login>
    {
        public override void BeforeStartUp()
        {
            this.Library.PopulateMembers(this.Username, this.Password);
        }

        [Input]
        public string Username { get; set; }

        [Input]
        public string Password { get; set; }

        public override void HandleModel()
        {
            this.SessionId = this.Username;
            this.SessionKey = Session.AddSession(this.Username);
        }

        public override void MapModel()
        {
            this.SessionId = this.Username;
            this.SessionKey = this.SessionKey;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return TfValidationResult.FieldRequired(nameof(this.Username), this.Username);

            if (this.Mapping)
            {
                yield return TfValidationResult.FieldRequired(nameof(this.SessionKey), this.SessionKey);
                if (this.IsValid(nameof(this.Username), nameof(this.SessionKey)))
                {
                    yield return TfValidationResult.CheckSessionActivity(this.Username, this.SessionKey);
                }
            }

            if (this.Handling)
            {
                yield return TfValidationResult.FieldRequired(nameof(this.Password), this.Password);
                if (this.IsValid(nameof(this.Username), nameof(this.Password)))
                {
                    yield return this.Library.CheckUsernamePassword(nameof(this.Username), nameof(this.Password));
                }
            }
        }
    }
}
