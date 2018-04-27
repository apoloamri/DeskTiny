using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools.Extensions;
using TenderfootCampaign.Library._Common;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Library.Campaign
{
    public class GetBonus
    {
        public string CampaignCode { get; set; }
        public Schema<Campaigns> Campaign { get; set; } = _DB.Campaigns;
        public Schema<Bonuses> Bonuses { get; set; } = _DB.Bonuses;

        public ValidationResult ValidatePoints(string sessionId, params string[] memberNames)
        {
            var memberPoints = TokensStrategy.GetPoints(sessionId);
            if (this.Campaign.SelectToEntity() == null)
            {
                return TfValidationResult.Compose("NotExists", memberNames);
            }
            if (this.Campaign.Entity.price > memberPoints)
            {
                return TfValidationResult.Compose("InsufficientPoints", memberNames);
            }
            return null;
        }
        
        public void GetCampaignCode(string sessionId)
        {
            var members = _DB.Members;

            this.Bonuses.Relate(Join.INNER, members,
                members.Relation(members._(x => x.id), this.Bonuses._(x => x.member_id)));
            this.Bonuses.Relate(Join.INNER, this.Campaign,
                 this.Campaign.Relation(this.Campaign._(x => x.campaign_code), this.Bonuses._(x => x.campaign_code)));

            this.Bonuses.Case.Where(members._(x => x.username), Is.EqualTo, Encryption.Decrypt(sessionId));
            this.Bonuses.Case.OrderBy(this.Bonuses._(x => x.id), Order.DESC);

            this.CampaignCode = this.Bonuses.Select.Entity?.campaign_code ?? string.Empty;
        }

        public Dictionary<string, object> GetCurrentLevel()
        {
            if (this.CampaignCode.IsEmpty())
            {
                return null;
            }
            var dictionary = new Dictionary<string, object>();
            var campaigns = this.Bonuses.Select.EntitiesAs<Campaigns>();
            dictionary["level"] = campaigns.FirstOrDefault().level;
            dictionary["discount"] = campaigns.Sum(x => x.bonus_percent);
            dictionary["multiplier"] = campaigns.Sum(x => x.bonus_multiplier);
            dictionary["item"] = campaigns.Sum(x => x.item_count);
            return dictionary;
        }
        
        public List<dynamic> GetNextLevel()
        {
            this.Campaign.Clear();
            if (this.CampaignCode.IsEmpty())
            {
                this.Campaign.Entity.level = 1;
            }
            else
            {
                this.Campaign.Case.Where(this.Campaign._(x => x.prerequisite), Is.EqualTo, this.CampaignCode);
            }
            return this.Campaign.Select.Result;
        }

        public void PurchaseBonus(string sessionId)
        {
            var memberId = MemberStrategy.GetMemberFromSession(sessionId)?.id;
            if (memberId == null)
            {
                return;
            }
            this.Bonuses.Entity.member_id = memberId;
            this.Bonuses.Insert();
            TokensStrategy.Add(memberId, -this.Campaign.Entity.price);
        }
    }
}
