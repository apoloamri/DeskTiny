using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class OrderHeaders : Entity
    {
        [NotNull]
        public int? member_id { get; set; }

        [NotNull]
        [Default("0")]
        public int? price { get; set; }

        [NotNull]
        [Default("0")]
        public int? points { get; set; }
    }
}
