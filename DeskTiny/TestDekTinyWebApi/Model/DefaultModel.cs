using DeskTiny.Database;
using DeskTiny.Mvc.Attributes;
using System.Collections.Generic;

namespace TestDekTinyWebApi.Model
{
    public class DefaultModel
    {
        [ShowAtJsonResult]
        public List<Dictionary<string, object>> dictionary { get; set; }

        public void GetClients()
        {
            var clients = Schemas.Clients();

            this.dictionary = clients.Select.Dictionaries();
        }
    }
}
