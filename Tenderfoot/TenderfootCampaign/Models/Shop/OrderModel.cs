﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library.Shop;

namespace TenderfootCampaign.Models.Shop
{
    public class OrderModel : TfModel<Order>
    {
        public override void BeforeStartUp()
        {
            this.Library.Carts.Entity.SetValuesFromModel(this);
        }

        public override void HandleModel()
        {
            
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
