using Tenderfoot.Mvc;
using TenderfootMessenger.DT.Database;
using TenderfootMessenger.DT.Members;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TenderfootMessenger.Models.Member
{
    public class RegisterModel : TfModel
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

        public override void HandleModel()
        {
            var member = Schemas.Members;
            member.Entity.username = this.Username;
            member.Entity.password = this.Password;
            member.Entity.email = this.Email;
            member.Entity.first_name = this.FirstName;
            member.Entity.last_name = this.LastName;
            member.Insert();
        }

        public override void MapModel()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Password != this.ConfirmPassword)
            {
                yield return TfValidationResult.Compose("ConfirmPassword", nameof(this.Password), nameof(this.ConfirmPassword));
            }
            
            if (CheckMember.CheckEmailExists(this.Email))
            {
                yield return TfValidationResult.Compose("EmailExists", nameof(this.Email));
            }

            if (CheckMember.CheckUsernameExists(this.Username))
            {
                yield return TfValidationResult.Compose("UsernameExists", nameof(this.Username));
            }

            yield return TfValidationResult.FieldRequired(nameof(this.Username), this.Username);
            yield return TfValidationResult.FieldRequired(nameof(this.Password), this.Password);
            yield return TfValidationResult.FieldRequired(nameof(this.ConfirmPassword), this.ConfirmPassword);
            yield return TfValidationResult.FieldRequired(nameof(this.Email), this.Email);
            yield return TfValidationResult.FieldRequired(nameof(this.FirstName), this.FirstName);
            yield return TfValidationResult.FieldRequired(nameof(this.LastName), this.LastName);
        }
    }
}
