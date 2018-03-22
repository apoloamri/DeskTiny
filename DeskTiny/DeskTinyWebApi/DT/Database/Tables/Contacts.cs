using DTCore.Database;
using DTCore.Database.Attributes;

namespace DeskTinyWebApi.DT.Database.Tables
{
    public class Contacts : Entity
    {
        [NotNull]
        public int member_id { get; set; }

        [NotNull]
        public int contact_id { get; set; }
    }
}
