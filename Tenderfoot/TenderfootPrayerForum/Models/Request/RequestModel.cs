using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.Net;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;
using TenderfootPrayerForum.Database;

namespace TenderfootPrayerForum.Models.Request
{
    public class RequestModel : TfModel
    {
        [Input]
        public string Fullname { get; set; }

        [Input]
        [RequireInput]
        public Requests Request { get; set; }

        public override void BeforeStartUp()
        {
            var name = this.Fullname?.Split(',');
            if (this.Request != null && name != null)
            {
                this.Request.last_name = name.Length >= 1 ? name[0].Trim().ToTitleCase() : string.Empty;
                this.Request.first_name = name.Length >= 2 ? name[1].Trim().ToTitleCase() : string.Empty;
                this.Request.request_type = 1;
                this.Request.shared = 0;
            }
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void HandleModel()
        {
            var requests = _DB.Requests;
            do
            {
                var uniqueKey = KeyGenerator.GetUniqueKey(64);
                requests.ClearCase();
                requests.Entity.activation_key = uniqueKey;
                requests.Entity.active = 0;
            }
            while (requests.Count > 0);
            requests.Entity.SetValuesFromModel(this.Request, false);
            requests.Insert();
            TfEmail.Send(this.Request.email, "request_email", requests.Entity.ToDictionary());
        }
    }
}