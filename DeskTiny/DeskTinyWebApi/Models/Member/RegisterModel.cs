using DeskTinyWebApi.DT.Database;
using DeskTinyWebApi.DT.Members;
using DTCore.Mvc;
using DTCore.Mvc.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeskTinyWebApi.Models.Member
{
    public class RegisterModel : DTModel
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
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Password != this.ConfirmPassword)
            {
                yield return DTValidationResult.Compose("ConfirmPassword", nameof(this.Password), nameof(this.ConfirmPassword));
            }
            
            if (CheckMember.CheckEmailExists(this.Email))
            {
                yield return DTValidationResult.Compose("EmailExists", nameof(this.Email));
            }

            if (CheckMember.CheckUsernameExists(this.Username))
            {
                yield return DTValidationResult.Compose("UsernameExists", nameof(this.Username));
            }

            yield return DTValidationResult.FieldRequired(nameof(this.Username), this.Username);
            yield return DTValidationResult.FieldRequired(nameof(this.Password), this.Password);
            yield return DTValidationResult.FieldRequired(nameof(this.ConfirmPassword), this.ConfirmPassword);
            yield return DTValidationResult.FieldRequired(nameof(this.Email), this.Email);
            yield return DTValidationResult.FieldRequired(nameof(this.FirstName), this.FirstName);
            yield return DTValidationResult.FieldRequired(nameof(this.LastName), this.LastName);
        }
    }
}
