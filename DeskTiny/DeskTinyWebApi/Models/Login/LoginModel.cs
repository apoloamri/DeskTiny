﻿using DeskTinyWebApi.DT.Members;
using DTCore.Mvc;
using DTCore.Mvc.Attributes;
using DTCore.WebApi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeskTinyWebApi.Models.Login
{
    public class LoginModel : DTModel
    {
        [Input]
        public string Username { get; set; }
        
        [Input]
        public string Password { get; set; }
        
        [JsonProperty]
        public bool? LoggedIn { get; set; }
        
        public override void MapModel()
        {
            this.LoggedIn = Session.IsSessionActive(this.Username, this.SessionKey);
        }

        public override void HandleModel()
        {
            this.SessionKey = Session.AddSession(this.Username);
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return DTValidationResult.FieldRequired(nameof(this.Username), this.Username);

            if (this.Mapping)
            {
                yield return DTValidationResult.FieldRequired(nameof(this.SessionKey), this.SessionKey);
            }
            
            if (this.Handling)
            {
                yield return DTValidationResult.FieldRequired(nameof(this.Password), this.Password);

                if (!CheckMember.CheckUsernamePasswordExists(this.Username, this.Password))
                {
                    yield return DTValidationResult.Compose("InvalidLogin", nameof(this.Username), nameof(this.Password));
                }
            }
        }
    }
}
