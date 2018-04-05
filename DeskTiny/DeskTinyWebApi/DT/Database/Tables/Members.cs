using DTCore.Database;
using DTCore.Database.Attributes;

namespace DTMessenger.DT.Database.Tables
{
    public class Members : Entity
    {
        [NotNull]
        [Length(50)]
        public string username { get; set; }

        [NotNull]
        [Length(100)]
        public string password { get; set; }
        
        [NotNull]
        [Length(100)]
        public string email { get; set; }

        [NotNull]
        [Length(100)]
        public string first_name { get; set; }

        [NotNull]
        [Length(100)]
        public string last_name { get; set; }
        
        [NotNull]
        [Default("0")]
        public int? gender { get; set; }
    }
}
