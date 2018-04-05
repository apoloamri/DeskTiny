using DTCore.Database.Attributes;
using System;

namespace DTCore.Database.Tables
{
    public class Clients : Entity
    {
        [NotNull]
        [Length(100)]
        public string username { get; set; }

        [NotNull]
        [Length(100)]
        public string password { get; set; }
    }
}
