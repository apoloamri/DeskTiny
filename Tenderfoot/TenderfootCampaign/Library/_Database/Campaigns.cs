using System;
using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class Campaigns : Entity
    {
        [NotNull]
        [Default("1")]
        public int? level { get; set; }
        
        [NotNull]
        public string campaign_code { get; set; }
        
        public string[] prerequisite { get; set; }

        [NotNull]
        [Default("0")]
        public int? price { get; set; }

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
