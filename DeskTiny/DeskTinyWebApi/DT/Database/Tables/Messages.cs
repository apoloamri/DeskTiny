using DTCore.Database;
using DTCore.Database.Attributes;

namespace DTMessenger.DT.Database.Tables
{
    public class Messages : Entity
    {
        [NotNull]
        public string sender { get; set; }

        [NotNull]
        public string recipient { get; set; }

        [NotNull]
        [Text]
        public string message { get; set; }

        [NotNull]
        [Default("0")]
        public short? unread { get; set; }
    }
}
