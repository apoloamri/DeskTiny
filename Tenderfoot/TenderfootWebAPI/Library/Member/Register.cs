using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootWebAPI.Library._Database;

namespace TenderfootWebAPI.Library.Member
{
    public class Register
    {
        public ValidationResult ConfirmEmail(string email, string memberName)
        {
            var members = _DB.Members;
            members.Conditions.Where(members.Column(x => x.email), Is.EqualTo, email);
            return
                members.HasRecords() ?
                TfValidationResult.Compose("InvalidExistance", new[] { email }, memberName) :
                null;
        }

        public ValidationResult ConfirmPassword(string pass, string pass2, params string[] memberNames)
        {
            return
                pass != pass2 ?
                TfValidationResult.Compose("ConfirmPassword", memberNames) :
                null;
        }

        public ValidationResult ConfirmUsername(string username, string memberName)
        {
            var members = _DB.Members;
            members.Conditions.Where(members.Column(x => x.username), Is.EqualTo, username);
            return
                members.HasRecords() ?
                TfValidationResult.Compose("InvalidExistance", new[] { username }, memberName) :
                null;
        }
    }
}
