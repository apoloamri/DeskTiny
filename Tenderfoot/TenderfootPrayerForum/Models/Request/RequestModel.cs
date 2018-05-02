using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.Net;
using Tenderfoot.Tools;
using TenderfootPrayerForum.Database;

namespace TenderfootPrayerForum.Models.Request
{
    public class RequestModel : TfModel
    {
        [Input]
        public Requests Request { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void HandleModel()
        {
            var requests = _DB.Requests;
            requests.Entity.SetValuesFromModel(this.Request);
            requests.Entity.activation_key = KeyGenerator.GetUniqueKey(16);
            requests.Insert();
            TfEmail.Send(this.Request.email, "request_email", requests.Entity.ToDictionary());
        }
    }
}