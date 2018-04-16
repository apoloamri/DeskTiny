using Tenderfoot.Database;

namespace TenderfootPrayerForum.Library.Database
{
    public class _DB : Schemas
    {
        public static Schema<Members> Members => CreateTable<Members>("members");
    }
}
