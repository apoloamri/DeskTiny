using System;

namespace DTCore.Database.Tables
{
    public class Sessions : Entity
    {
        public string session_id { get; set; }
        public string session_key { get; set; }
        public DateTime? session_time { get; set; }
    }
}
