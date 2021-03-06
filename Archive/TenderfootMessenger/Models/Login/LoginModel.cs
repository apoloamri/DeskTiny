﻿using Tenderfoot.Mvc;
using TenderfootMessenger.DT.Members;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TenderfootMessenger.Models.Login
{
    public class LoginModel : TfModel
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
            yield return TfValidationResult.FieldRequired(nameof(this.Username), this.Username);

            if (this.Mapping)
            {
                yield return TfValidationResult.FieldRequired(nameof(this.SessionKey), this.SessionKey);
            }
            
            if (this.Handling)
            {
                yield return TfValidationResult.FieldRequired(nameof(this.Password), this.Password);

                if (!CheckMember.CheckUsernamePasswordExists(this.Username, this.Password))
                {
                    yield return TfValidationResult.Compose("InvalidLogin", nameof(this.Username), nameof(this.Password));
                }
            }
        }
    }
}
