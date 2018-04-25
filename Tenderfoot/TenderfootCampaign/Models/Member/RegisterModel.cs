using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.Tools;
using TenderfootCampaign.Library._Database;
using TenderfootCampaign.Library.Member;

namespace TenderfootCampaign.Models.Member
{
    public class RegisterModel : TfModel<Register>
    {
        [Input]
        public string Username { get; set; }

        [Input]
        public string Password { get; set; }

        [Input]
        public string ConfirmPassword { get; set; }

        [Input]
        public string Email { get; set; }

        [Input]
        public string FirstName { get; set; }

        [Input]
        public string LastName { get; set; }

        [Input]
        public int? Gender { get; set; }

        public override void HandleModel()
        {
            var activationKey = KeyGenerator.GetUniqueKey(10);
            var members = _DB.Members;

            members.Entity.SetValuesFromModel(this);
            members.Entity.active = 1;
            members.Insert();
        }

        public override void MapModel()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Handling)
            {
                yield return this.FieldRequired(nameof(this.Username));
                yield return this.FieldRequired(nameof(this.Password));
                yield return this.FieldRequired(nameof(this.ConfirmPassword));
                yield return this.FieldRequired(nameof(this.Email));
                yield return this.FieldRequired(nameof(this.FirstName));
                yield return this.FieldRequired(nameof(this.LastName));
                yield return this.FieldRequired(nameof(this.Gender));
                if (this.IsValid(nameof(this.Email)))
                {
                    yield return this.Library.ConfirmEmail(this.Email, nameof(this.Email));
                }
                if (this.IsValid(nameof(this.Username)))
                {
                    yield return this.Library.ConfirmUsername(this.Username, nameof(this.Username));
                }
                if (this.IsValid(nameof(this.Password), nameof(this.ConfirmPassword)))
                {
                    yield return this.Library.ConfirmPassword(this.Password, this.ConfirmPassword, nameof(this.Password), nameof(this.ConfirmPassword));
                }   
            }
        }
    }
}
