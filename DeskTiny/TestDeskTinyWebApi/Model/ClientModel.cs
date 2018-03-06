using DeskTiny.Database.Enums;
using DeskTiny.Mvc.CustomAttributes;
using System.Collections.Generic;
using TestDekTinyWebApi.Library;

namespace TestDekTinyWebApi.Model
{
    public class ClientModel
    {
        [JsonResult]
        public List<Dictionary<string, object>> MemberList { get; set; }

        public long? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        public void RegisterClient()
        {
            var client = Factory.Clients;

            client.Entity.username = this.Username;
            client.Entity.password = this.Password;

            client.Insert();
        }

        public void UpdateClient()
        {
            var client = Factory.Clients;

            client.Entity.id = this.Id;
            client.Entity.username = this.Username;
            client.Entity.password = this.Password;
            client.Conditions.AddWhere(nameof(client.Entity.id), Condition.Equal, this.Id);

            client.Update();
        }

        public void GetRegisteredClients()
        {
            var client = Factory.Clients;

            this.MemberList = client.Select.Dictionaries;
        }
    }
}
