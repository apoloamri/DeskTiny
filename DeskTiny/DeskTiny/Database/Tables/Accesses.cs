using DTCore.Database.Attributes;
using System;

namespace DTCore.Database.Tables
{
    public class Accesses : Entity
    {
        [NotNull]
        public string token { get; set; }

        [NotNull]
        public string token_secret { get; set; }
    }
}
