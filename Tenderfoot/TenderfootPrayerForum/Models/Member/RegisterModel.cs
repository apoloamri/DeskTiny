using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Library.Database;

namespace TenderfootPrayerForum.Models.Member
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

        [Input]
        public int? Gender { get; set; }

        public override void HandleModel()
        {
            var members = _DB.Members;
            members.Entity.SetValuesFromModel(this);
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
                yield return DTValidationResult.FieldRequired(nameof(this.Username), this.Username);
                yield return DTValidationResult.FieldRequired(nameof(this.Password), this.Password);
                yield return DTValidationResult.FieldRequired(nameof(this.ConfirmPassword), this.ConfirmPassword);
                yield return DTValidationResult.FieldRequired(nameof(this.Email), this.Email);
                yield return DTValidationResult.FieldRequired(nameof(this.FirstName), this.FirstName);
                yield return DTValidationResult.FieldRequired(nameof(this.LastName), this.LastName);
                yield return DTValidationResult.FieldRequired(nameof(this.Gender), this.Gender);
            }
        }
    }
}
