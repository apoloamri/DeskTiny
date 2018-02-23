using System;

namespace DeskTiny.Database.Tables
{
    public class Affiliate : Repository<AffiliateEntity>
    {
        public Affiliate() : base("affiliate_t") { }
    }

    public class AffiliateEntity
    {
        public int? id { get; set; }
        public int? site_id { get; set; }
        public string business_client_code { get; set; }
        public string pr_tfaf { get; set; }
        public DateTime? intime { get; set; }
    }
}
