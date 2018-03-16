﻿using DeskTinyWebApi.DT.Database;
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
        
        public override void HandleModel()
        {
            var member = Schemas.Members;

            member.Entity.username = this.Username;
            member.Entity.password = this.Password;

            member.Insert();
        }

        public override void MapModel()
        {
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return DTValidationResult.FieldRequired(nameof(this.Username), this.Username);
            yield return DTValidationResult.FieldRequired(nameof(this.Password), this.Password);
        }
    }
}
