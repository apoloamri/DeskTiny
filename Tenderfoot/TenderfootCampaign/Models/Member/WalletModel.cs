using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Common;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Models.Member
{
    public class WalletModel : TfModel
    {
        private Schema<Wallets> Wallets { get; set; } = _DB.Wallets;
        private Members Member { get; set; }

        [Input]
        [JsonProperty]
        [ValidateInput(InputType.Number)]
        public int? Amount { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.ValidateSession();

            if (this.Handling)
            {
                yield return this.FieldRequired(nameof(this.Amount));
            }
        }

        public override void OnStartUp()
        {
            this.Member = MemberStrategy.GetMemberFromSession(this.SessionId);
            if (this.Member == null)
            {
                this.StopProcess();
            }
        }

        public override void MapModel()
        {   
            this.Wallets.Entity.member_id = this.Member.id;
            this.Amount = MemberStrategy.GetMemberWallet(this.Member.id)?.amount ?? 0;
        }

        public override void HandleModel()
        {
            MemberStrategy.UpdateMemberWallet(this.Member.id, this.Amount ?? 0);
        }
    }
}