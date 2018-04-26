using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Common;

namespace TenderfootCampaign.Models.Member
{
    public class PointsModel : TfModel
    {
        [JsonProperty]
        public int TotalPoints { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.CheckSessionActivity();
        }

        public override void MapModel()
        {
            this.TotalPoints = TokensStrategy.GetPoints(this.SessionId);
        }
    }
}
