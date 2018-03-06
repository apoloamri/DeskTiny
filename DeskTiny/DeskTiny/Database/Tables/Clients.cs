using System;

namespace DeskTiny.Database.Tables
{
    public class Clients : Entity
    {
        public override int? id { get; set; }
        public override DateTime? insert_time { get; set; }

        public string username { get; set; }
        public string password { get; set; }
    }
}
