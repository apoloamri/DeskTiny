using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Database;

namespace TenderfootPrayerForum.Models.Member
{
    public class ActivateModel : TfModel
    {
        [Input]
        [RequireInput]
        public string Key { get; set; }

        public Schema<Members> Member { get; set; } = _DB.Members;

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValid(nameof(this.Key)))
            {
                this.Member.Entity.activation_key = this.Key;
                this.Member.Entity.active = 0;
                if (!this.Member.HasRecord)
                {
                    yield return TfValidationResult.Compose("InvalidActivationKey", nameof(this.Key));
                }
            }
        }

        public override void HandleModel()
        {
            this.Member.Entity.active = 1;
            this.Member.Update();
        }
    }
}