using DTCore.Database;
using DTCore.Database.Attributes;

namespace DTMessenger.DT.Database.Tables
{
    public class Members : Entity
    {
        [NotNull]
        public string username { get; set; }

        [NotNull]
        public string password { get; set; }
        
        [NotNull]
        public string email { get; set; }

        [NotNull]
        public string first_name { get; set; }

        [NotNull]
        public string last_name { get; set; }
    }
}
