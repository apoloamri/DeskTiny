using DTCore.Database.Attributes;
using System;

namespace DTCore.Database.Tables
{
    public class Clients : Entity
    {
        [NotNull(true)]
        [Length(100)]
        public string username { get; set; }

        [NotNull(true)]
        [Length(100)]
        public string password { get; set; }

        public DateTime? update_time { get; set; }

        [Default("paolotest")]
        public string paolotest { get; set; }
    }
}
