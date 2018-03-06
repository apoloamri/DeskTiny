using System;

namespace DeskTiny.Database.Tables
{
    public class Accesses : Entity
    {
        public override int? id { get; set; }
        public override DateTime? insert_time { get; set; }

        public string token { get; set; }
        public string token_secret { get; set; }
    }
}
