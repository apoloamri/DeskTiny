using DTCore.Database;

namespace DTMessenger.DT.Database.Tables
{
    public class Groups : Entity
    {
        [NotNull]
        [Length(50)]
        public string name { get; set; }

        [NotNull]
        [Length(50)]
        public string creator { get; set; }

        [NotNull]
        [Length(50)]
        public string[] members { get; set; }
    }
}
