using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Library.Home
{
    public class Login
    {
        public Schema<Members> Members { get; set; } = _DB.Members;

        public void PopulateMembers(string username, string password)
        {
            this.Members.Case.Where(this.Members._("username"), Is.EqualTo, username);
            this.Members.Case.Where(this.Members._("password"), Is.EqualTo, password);
        }

        public ValidationResult CheckUsernamePassword(params string[] memberNames)
        {
            if (this.Members.Count == 0)
            {
                return TfValidationResult.Compose("InvalidLogin", memberNames);
            }

            return this.CheckActivity(memberNames);
        }

        public ValidationResult CheckActivity(params string[] memberNames)
        {
            this.Members.Case.Where(this.Members._("active"), Is.EqualTo, 1);
            return
                this.Members.Count == 0 ?
                TfValidationResult.Compose("InactiveAccount", memberNames) :
                null;
        }
    }
}
