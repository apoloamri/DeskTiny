using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library.Campaign;

namespace TenderfootCampaign.Models.Campaign
{
    public class BonusModel : TfModel<GetBonus>
    {
        [Input]
        public string CampaignCode { get; set; }

        [JsonProperty]
        public Dictionary<string, object> CurrentOptions { get; set; }

        [JsonProperty]
        public List<dynamic> NextOptions { get; set; }
        
        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.CheckSessionActivity();

            if (this.Handling)
            {
                yield return this.FieldRequired(nameof(this.CampaignCode));
                if (this.IsValid(nameof(this.CampaignCode)))
                {
                    this.Library.Campaign.Entity.SetValuesFromModel(this);
                    yield return this.Library.ValidatePoints(this.SessionId);
                }
            }
        }

        public override void MapModel()
        {
            this.Library.GetCampaignCode(this.SessionId);
            this.CurrentOptions = this.Library.GetCurrentLevel();
            this.NextOptions = this.Library.GetNextLevel();
        }

        public override void HandleModel()
        {
            this.Library.Bonuses.Entity.SetValuesFromModel(this);
            this.Library.PurchaseBonus(this.SessionId);
        }
    }
}
