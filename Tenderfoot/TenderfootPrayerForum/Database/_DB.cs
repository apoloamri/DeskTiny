using Tenderfoot.Database;

namespace TenderfootPrayerForum.Database
{
    public class _DB : Schemas
    {
        public static Schema<Members> Members => CreateTable<Members>("members");
        public static Schema<Posts> Posts => CreateTable<Posts>("posts");
        public static Schema<Requests> Requests => CreateTable<Requests>("requests");
    }
}
