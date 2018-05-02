using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class Wallets : Entity
    {
        [NotNull]
        public int? member_id { get; set; }

        [NotNull]
        [Default("0")]
        public int? amount { get; set; }
    }
}