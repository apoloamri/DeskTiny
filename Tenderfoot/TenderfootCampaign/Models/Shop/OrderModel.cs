using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;
using TenderfootCampaign.Library.Shop;

namespace TenderfootCampaign.Models.Shop
{
    public class OrderModel : TfModel<Order>
    {
        public override void OnStartUp()
        {
            this.Library.GetCartItems();
        }

        public override void BeforeStartUp()
        {
            this.Library.Carts.Entity.SetValuesFromModel(this);
        }

        public override void HandleModel()
        {
            var orders = _DB.Orders;
            foreach (var item in this.Library.CartItems)
            {
                orders.Entity.member_id = (int)item.id1;
                orders.Entity.item_code = item.item_code;
                orders.Entity.amount = (int)item.amount;
                orders.Entity.total_price = (int)item.price * (int)item.amount;
                orders.Entity.points_earned = 0;
                orders.Insert();
            }
        }

        public override void MapModel()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.CheckSessionActivity();
            yield return this.Library.ValidateCart(nameof(this.SessionKey));
        }
    }
}
