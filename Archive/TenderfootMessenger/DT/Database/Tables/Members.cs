﻿using Tenderfoot.Database;

namespace TenderfootMessenger.DT.Database.Tables
{
    public class Members : Entity
    {
        [NotNull]
        [Length(50)]
        [Encrypt]
        public string username { get; set; }

        [NotNull]
        [Length(100)]
        [Encrypt]
        public string password { get; set; }
        
        [NotNull]
        [Length(100)]
        [Encrypt]
        public string email { get; set; }

        [NotNull]
        [Length(100)]
        [Encrypt]
        public string first_name { get; set; }

        [NotNull]
        [Length(100)]
        [Encrypt]
        public string last_name { get; set; }
        
        [NotNull]
        [Default("0")]
        public int? gender { get; set; }
    }
}
