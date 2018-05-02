using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Common;
using TenderfootCampaign.Library.Shop;

namespace TenderfootCampaign.Models.Shop
{
    public class OrderModel : TfModel<Order>
    {
        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.ValidateSession();

            if (this.IsValid(nameof(this.SessionId), nameof(this.SessionKey)))
            {
                this.Library.Member = MemberStrategy.GetMemberFromSession(this.SessionId);
                this.Library.Carts.Entity.SetValuesFromModel(this);
                yield return this.Library.ValidateCart(nameof(this.SessionKey));
                this.Library.GetCartItems();
                yield return this.Library.ValidateWallet(this.SessionId, nameof(this.SessionId));
            }
        }
        
        public override void HandleModel()
        {
            this.Library.ApplyDiscounts(this.SessionId);
            this.Library.DeleteCart();
            this.Library.ExecuteOrder();
        }
    }
}
