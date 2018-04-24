using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class Bonus : Entity
    {
        [NotNull]
        public string campaign_code { get; set; }

        [NotNull]
        public int? member_id { get; set; }
        
        [NotNull]
        [Default("1")]
        public int? active { get; set; }
    }
}
