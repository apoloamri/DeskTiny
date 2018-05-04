using System;
using Tenderfoot.Database;

namespace TenderfootPrayerForum.Database
{
    public class Members : Entity
    {
        [NotNull]
        [Length(100)]
        [Encrypt]
        public string email { get; set; }

        [NotNull]
        [Length(100)]
        [Encrypt]
        public string password { get; set; }

        [NotNull]
        [Length(100)]
        [Encrypt]
        public string first_name { get; set; }

        [NotNull]
        [Length(100)]
        [Encrypt]
        public string last_name { get; set; }

        [NotNull]
        public DateTime? birthdate { get; set; }

        [NotNull]
        [Default("0")]
        public int? gender { get; set; }

        [NotNull]
        [Default("0")]
        public int? active { get; set; }
    }
}
