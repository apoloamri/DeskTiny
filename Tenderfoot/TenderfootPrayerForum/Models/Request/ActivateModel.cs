using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.Net;
using Tenderfoot.TfSystem;
using TenderfootPrayerForum.Database;

namespace TenderfootPrayerForum.Models.Request
{
    public class ActivateModel : TfModel
    {
        [Input]
        [RequireInput]
        public string Key { get; set; }

        public Schema<Requests> Request { get; set; } = _DB.Requests;

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValid(nameof(this.Key)))
            {
                this.Request.Entity.activation_key = this.Key;
                this.Request.Entity.active = 0;
                if (!this.Request.HasRecord)
                {
                    yield return TfValidationResult.Compose("InvalidActivationKey", nameof(this.Key));
                }
            }
        }

        public override void HandleModel()
        {
            this.Request.Entity.active = 1;
            this.Request.Update();
            this.Request.ClearCase();
            this.Request.SelectToEntity();
            TfEmail.Send(
                TfSettings.GetSettings("PrayerForum", "ChurchEmail").ToString(), 
                "request_activate_email", 
                this.Request.Entity.ToDictionary());
        }
    }
}