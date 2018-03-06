using DeskTiny.Database.Enums;
using DeskTiny.Mvc.CustomAttributes;
using DeskTiny.WebApi;
using DeskTinyWebApi.DT.Database;

namespace DeskTinyWebApi.Models.Login
{
    public class LoginModel : DeskTiny.Mvc.CustomModel
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
