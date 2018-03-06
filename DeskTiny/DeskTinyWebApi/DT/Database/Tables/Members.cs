using System;

namespace DeskTinyWebApi.DT.Database.Tables
{
    public class Members : DeskTiny.Database.Entity
    {
        public override int? id { get; set; }
        public override DateTime? insert_time { get; set; }

        public string username { get; set; }
        public string password { get; set; }
    }
}
