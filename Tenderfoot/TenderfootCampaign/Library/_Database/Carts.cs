using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class Carts : Entity
    {
        [NotNull]
        public string session_key { get; set; }

        [NotNull]
        public string item_code { get; set; }

        [NotNull]
        public int? amount { get; set; }
    }
}
