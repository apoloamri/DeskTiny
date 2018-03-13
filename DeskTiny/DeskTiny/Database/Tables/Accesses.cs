using System;

namespace DTCore.Database.Tables
{
    public class Accesses : Entity
    {
        public string token { get; set; }
        public string token_secret { get; set; }
    }
}
