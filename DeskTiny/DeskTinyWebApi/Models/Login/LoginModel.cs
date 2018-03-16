using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DeskTinyWebApi.DT.Database;
using DTCore.Database.Enums;
using DTCore.Mvc.Attributes;
using DTCore.WebApi;

namespace DeskTinyWebApi.Models.Login
{
    public class LoginModel : DTCore.Mvc.DTModel
    {
        [Input]
        public string Username { get; set; }
        
        [Input]
        public string Password { get; set; }

        [Input]
        [JsonProperty]
        public string SessionKey { get; set; }

        [JsonProperty]
        public bool? LoggedIn { get; set; }
        
        internal void IsLogin()
        {
            this.LoggedIn = Session.IsSessionActive(this.Username, this.SessionKey);
        }

        internal void Login()
        {
            var members = Schemas.Members;

            members.Wherein.Which(
                nameof(members.Entity.username), 
                Condition.EqualTo, 
                this.Username);

            members.Wherein.Which(
                nameof(members.Entity.password), 
                Condition.EqualTo, 
                this.Password);
            
            if (members.Count() == 1)
            {
                this.SessionKey = Session.AddSession(this.Username);
            }
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (string.IsNullOrEmpty(this.Username))
            {
                yield return new ValidationResult("Username is a required field.", new[] { nameof(this.Username) });
            }

            if (this.HttpMethod == DTCore.Mvc.Enums.HttpMethod.GET)
            {
                if (string.IsNullOrEmpty(this.SessionKey))
                {
                    yield return new ValidationResult("SessionKey is a required field.", new[] { nameof(this.SessionKey) });
                }
            }

            if (this.HttpMethod == DTCore.Mvc.Enums.HttpMethod.POST)
            {
                if (string.IsNullOrEmpty(this.Password))
                {
                    yield return new ValidationResult("Password is a required field.", new[] { nameof(this.Password) });
                }
            }
        }

        public override void MapModel()
        {
            this.IsLogin();
        }

        public override void HandleModel()
        {
            this.Login();
        }
    }
}
