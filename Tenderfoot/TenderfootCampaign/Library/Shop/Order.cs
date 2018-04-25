using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Library.Shop
{
    public class Order
    {
        public Schema<Carts> Carts { get; set; } = _DB.Carts;

        public ValidationResult ValidateCart(params string[] memberNames)
        {
            if (!this.Carts.HasRecords)
            {
                return TfValidationResult.Compose("EmptyCart", memberNames);
            }
            else
            {
                foreach (var cart in this.Carts.Select.Entities)
                {
                    return AddCart.ValidateItem(cart.item_code, memberNames);
                }
            }

            return null;
        }
    }
}
