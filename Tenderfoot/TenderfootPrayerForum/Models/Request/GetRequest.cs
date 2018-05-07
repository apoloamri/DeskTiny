using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Database;

namespace TenderfootPrayerForum.Models.Request
{
    public class GetRequest
    {
        public Schema<Requests> Requests { get; set; } = _DB.Requests;
        public Schema<Members> Members { get; set; } = _DB.Members;
        public bool IsRepliable { get; set; } = false;

        public ValidationResult ValidateRequest()
        {
            this.Requests.Entity.SetValuesFromModel(this);
            if (!this.Requests.HasRecord)
            {
                return TfValidationResult.Compose("DataNotFound", nameof(this.Requests.Entity.id));
            }
            return null;
        }

        public ValidationResult ValidateMember()
        {
            if (this.Requests.HasRecord && this.Members.HasRecord)
            {
                var request = this.Requests.Select.Entity;
                var member = this.Members.Select.Entity;
                this.IsRepliable = new[] { (int?)EnumAccountType.Ministry, (int?)EnumAccountType.Admin }.Contains(member.account_type);

                if (request.email != member.email)
                {
                    if (!this.IsRepliable)
                    {
                        return TfValidationResult.Compose("DataNotFound", nameof(this.Requests.Entity.id));
                    }
                }
            }
            return null;
        }
    }
}