using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Database;

namespace TenderfootPrayerForum.Models.Member
{
    public class Register
    {
        public Schema<Members> Members { get; set; } = _DB.Members;

        public ValidationResult ConfirmEmail(string email, string memberName)
        {
            var members = _DB.Members;
            members.Entity.email = email;
            return
                members.HasRecords ?
                TfValidationResult.Compose("InvalidDuplicate", new[] { email }, memberName) :
                null;
        }
    }
}