using DTCore.Database;
using DTCore.Database.Attributes;

namespace DeskTinyWebApi.DT.Database.Tables
{
    public class Messages : Entity
    {
        [NotNull]
        public string sender { get; set; }

        [NotNull]
        public string recipient { get; set; }

        [NotNull]
        public string message { get; set; }
    }
}
