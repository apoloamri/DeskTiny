using DTCore.Database;
using DTCore.Database.Attributes;

namespace DTMessenger.DT.Database.Tables
{
    public class Contacts : Entity
    {
        [NotNull]
        [Length(50)]
        public string username { get; set; }

        [NotNull]
        [Length(50)]
        public string contact_username { get; set; }
    }
}
