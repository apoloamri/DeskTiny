using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenderfoot.Database;

namespace TenderfootBoxCampaign.Daabase
{
    public class Members : Entity
    {
        [NotNull]
        public string lname { get; set; }
    }
}
