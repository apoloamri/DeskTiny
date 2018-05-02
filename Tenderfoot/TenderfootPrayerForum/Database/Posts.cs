using Tenderfoot.Database;

namespace TenderfootPrayerForum.Database
{
    public class Posts : Entity
    {
        [NotNull]
        public int? member_id { get; set; }

        [NotNull]
        public string post { get; set; }
    }
}