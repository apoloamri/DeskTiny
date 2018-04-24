using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class Items : Entity
    {
        [NotNull]
        public string item_code { get; set; }

        [NotNull]
        public string name { get; set; }

        [NotNull]
        [Default("0")]
        public int? price { get; set; }

        [NotNull]
        [Default("0")]
        public int? product_type { get; set; }
    }
}
