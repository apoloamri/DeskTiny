using DTCore.Database.Attributes;

namespace DeskTinyWebApi.DT.Database.Tables
{
    public class Members : DTCore.Database.Entity
    {
        [NotNull]
        public string username { get; set; }

        [NotNull]
        public string password { get; set; }
    }
}
