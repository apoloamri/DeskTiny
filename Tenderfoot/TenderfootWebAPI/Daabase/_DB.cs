using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenderfoot.Database;

namespace TenderfootBoxCampaign.Daabase
{
    public class _DB : Schemas
    {
        public static Schema<Members> Members => CreateTable<Members>("member_t");
    }
}
