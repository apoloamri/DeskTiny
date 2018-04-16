using Tenderfoot.Database;

namespace TenderfootMessenger.DT.Database.Tables
{
    public class Messages : Entity
    {
        [NotNull]
        [Encrypt]
        public string sender { get; set; }

        [NotNull]
        [Encrypt]
        public string recipient { get; set; }

        [NotNull]
        [Text]
        [Encrypt]
        public string message { get; set; }

        [NotNull]
        [Default("0")]
        public short? unread { get; set; }
    }
}
