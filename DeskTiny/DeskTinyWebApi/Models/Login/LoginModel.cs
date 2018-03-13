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
        [JsonResult]
        public string SessionKey { get; set; }

        [JsonResult]
        public bool? LoggedIn { get; set; }

        internal void IsLogin()
        {
            this.LoggedIn = Session.IsSessionActive(this.Username, this.SessionKey);
        }

        internal void Login()
        {
            var members = Schemas.Members;

            members.Conditions.AddWhere(
                nameof(members.Entity.username), 
                Condition.Equal, 
                this.Username);

            members.Conditions.AddWhere(
                nameof(members.Entity.password), 
                Condition.Equal, 
                this.Password);
            
            if (members.Count() == 1)
            {
                this.SessionKey = Session.AddSession(this.Username);
            }
        }
    }
}
