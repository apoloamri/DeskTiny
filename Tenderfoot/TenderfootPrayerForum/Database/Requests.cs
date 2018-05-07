using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace TenderfootPrayerForum.Database
{
    public class Requests : Entity
    {
        [Input]
        [RequireInput]
        [ValidateInput(InputType.Email)]
        [NotNull]
        public string email { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.String)]
        [NotNull]
        public string last_name { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.String)]
        [NotNull]
        public string first_name { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.All)]
        [NotNull]
        public string message { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.Numeric)]
        [NotNull]
        [Default("1")]
        public int? request_type { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.Numeric)]
        [NotNull]
        [Default("0")]
        public int? shared { get; set; }

        [NotNull]
        public string activation_key { get; set; }

        [NotNull]
        [Default("0")]
        public int? active { get; set; }
    }
}