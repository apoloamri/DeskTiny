using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class Orders : Entity
    {
        [NotNull]
        public int? member_id { get; set; }

        [NotNull]
        public string item_code { get; set; }

        [NotNull]
        public int? amount { get; set; }

        [NotNull]
        [Default("0")]
        public int? total_price { get; set; }

        [NotNull]
        [Default("0")]
        public int? points_earned { get; set; }
    }
}
