using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootEtherWallet.DB;

namespace TenderfootEtherWallet.Models.Member
{
    public class LoginModel : TfModel
    {
        [Input]
        [RequireInput]
        [ValidateInput(InputType.Alphabet)]
        public string Username { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.String)]
        public string Password { get; set; }

        private Schema<Members> Members { get; set; } = _DB.Members;

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValid(
                nameof(this.Username),
                nameof(this.Password)))
            {
                this.Members.Entity.username = this.Username;
                this.Members.Entity.password = this.Password;
                if (!this.Members.HasRecord)
                {
                    yield return TfValidationResult.Compose(
                        "InvalidLogin", 
                        nameof(this.Username), 
                        nameof(this.Password));
                }
            }
        }

        public override void HandleModel()
        {
            this.NewSession(this.Username);
        }
    }
}