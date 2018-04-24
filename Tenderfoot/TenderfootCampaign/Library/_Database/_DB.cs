using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class _DB : Schemas
    {
        public _DB()
        {
            var bonus = Bonus;
            var campaign = Campaign;
            var items = Items;
            var members = Members;
            var tokens = Tokens;
            var carts = Carts;
        }

        public static Schema<Bonus> Bonus => CreateTable<Bonus>("bonus");
        public static Schema<Carts> Carts => CreateTable<Carts>("carts");
        public static Schema<Campaign> Campaign => CreateTable<Campaign>("campaign");
        public static Schema<Items> Items => CreateTable<Items>("items");
        public static Schema<Members> Members => CreateTable<Members>("members");
        public static Schema<Tokens> Tokens => CreateTable<Tokens>("tokens");
    }
}
