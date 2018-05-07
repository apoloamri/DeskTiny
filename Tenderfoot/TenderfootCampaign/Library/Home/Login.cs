using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Library.Home
{
    public class Login
    {
        public Schema<Members> Members { get; set; } = _DB.Members;
        
        public ValidationResult CheckUsernamePassword(params string[] memberNames)
        {
            return
                this.Members.Count == 0 ?
                TfValidationResult.Compose("InvalidLogin", memberNames) :
                null;
        }

        public ValidationResult CheckActivity(params string[] memberNames)
        {
            this.Members.Case.Where(this.Members._(x => x.active), Is.EqualTo, 1);
            return
                this.Members.Count == 0 ?
                TfValidationResult.Compose("InactiveAccount", memberNames) :
                null;
        }
    }
}
