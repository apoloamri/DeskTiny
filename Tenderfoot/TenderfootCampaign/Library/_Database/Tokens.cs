using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class Tokens : Entity
    {
        [NotNull]
        public int? member_id { get; set; }

        [NotNull]
        public int? add_points { get; set; }

        [NotNull]
        public int? total_points { get; set; }
    }
}
