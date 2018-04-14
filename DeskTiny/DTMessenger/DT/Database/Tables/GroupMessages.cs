using DTCore.Database;
using DTCore.Database.Attributes;

namespace DTMessenger.DT.Database.Tables
{
    public class GroupMessages : Entity
    {
        [NotNull]
        public int? group_id { get; set; }

        [NotNull]
        [Encrypt]
        public string sender { get; set; }

        [Text]
        [Encrypt]
        public string message { get; set; }
    }
}
