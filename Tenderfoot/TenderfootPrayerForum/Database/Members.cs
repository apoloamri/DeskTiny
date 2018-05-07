using System;
using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace TenderfootPrayerForum.Database
{
    public class Members : Entity
    {
        [Input]
        [RequireInput]
        [ValidateInput(InputType.Email)]
        [NotNull]
        [Length(100)]
        [Encrypt]
        public string email { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.All)]
        [NotNull]
        [Length(100)]
        [Encrypt]
        public string password { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.String)]
        [NotNull]
        [Length(100)]
        [Encrypt]
        public string first_name { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.String)]
        [NotNull]
        [Length(100)]
        [Encrypt]
        public string last_name { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.DateTime)]
        [NotNull]
        public DateTime? birthdate { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.Numeric)]
        [NotNull]
        [Default("0")]
        public int? gender { get; set; }
        
        [NotNull]
        public string activation_key { get; set; }

        [NotNull]
        [Default("0")]
        public int? account_type { get; set; }

        [NotNull]
        [Default("0")]
        public int? active { get; set; }
    }
}
