using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library.Shop;

namespace TenderfootCampaign.Models.Shop
{
    public class OrderModel : TfModel<Order>
    {
        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.CheckSessionActivity();

            if (this.IsValid(nameof(this.SessionId), nameof(this.SessionKey)))
            {
                this.Library.Carts.Entity.SetValuesFromModel(this);
                yield return this.Library.ValidateCart(nameof(this.SessionKey));
            }
        }
        
        public override void HandleModel()
        {
            this.Library.GetCartItems();
            this.Library.DeleteCart();
            this.Library.ExecuteOrder();
        }
    }
}
