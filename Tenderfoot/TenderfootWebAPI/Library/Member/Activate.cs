using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootWebAPI.Library._Database;

namespace TenderfootWebAPI.Library.Member
{
    public class Activate
    {
        public Schema<Members> Members { get; set; } = _DB.Members;

        public void PopulateMembers(string username, string key)
        {
            this.Members.Conditions.Where(this.Members.Column(x => x.username), Is.EqualTo, username);
            this.Members.Conditions.Where(this.Members.Column(x => x.activation_key), Is.EqualTo, key);
        }

        public ValidationResult CheckKey(params string[] memberNames)
        {
            if (!this.Members.HasRecords())
            {
                return TfValidationResult.Compose("InvalidActivationKey", memberNames);
            }

            return this.CheckActivity(memberNames);
        }

        public ValidationResult CheckActivity(params string[] memberNames)
        {
            return
                !this.Members.HasRecords() ?
                TfValidationResult.Compose("AlreadyActive", memberNames) :
                null;
        }
    }
}
