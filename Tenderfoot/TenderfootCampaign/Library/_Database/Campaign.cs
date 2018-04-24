using System;
using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class Campaign : Entity
    {
        [NotNull]
        public string campaign_code { get; set; }

        [NotNull]
        public int? prerequisite { get; set; }

        [NotNull]
        [Default("0")]
        public int? bonus_percent { get; set; }

        [NotNull]
        [Default("0")]
        public int? bonus_multiplier { get; set; }

        [NotNull]
        [Default("0")]
        public int? item_count { get; set; }

        [NonTableColumn]
        public override DateTime? insert_time { get => base.insert_time; set => base.insert_time = value; }
    }
}
