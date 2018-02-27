using DeskTiny.Database.System;
using System;

namespace DeskTiny.Database.Tables
{
    public class Affiliate
    {
        public int? id { get; set; }
        public int? site_id { get; set; }
        public string business_client_code { get; set; }
        public string pr_tfaf { get; set; }
        public DateTime? intime { get; set; }
        public string agency_identifier { get; set; }
    }
}
