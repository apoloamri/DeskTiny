using System;

namespace DeskTiny.Database.Tables
{
    public class Sessions : Entity
    {
        public override int? id { get; set; }
        public override DateTime? insert_time { get; set; }

        public string session_id { get; set; }
        public string session_key { get; set; }
        public DateTime? session_time { get; set; }
    }
}
