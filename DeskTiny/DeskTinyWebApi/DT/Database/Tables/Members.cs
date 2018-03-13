using DTCore.Database.Attributes;

namespace DeskTinyWebApi.DT.Database.Tables
{
    public class Members : DTCore.Database.Entity
    {
        [NotNull(true)]
        public string username { get; set; }

        [NotNull(true)]
        public string password { get; set; }
    }
}
