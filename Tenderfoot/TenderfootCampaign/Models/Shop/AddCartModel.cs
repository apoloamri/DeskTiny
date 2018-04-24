using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;
using TenderfootCampaign.Library.Shop;

namespace TenderfootCampaign.Models.Shop
{
    public class AddCartModel : TfModel<AddCart>
    {
        public override void BeforeStartUp()
        {
            this.Library.PopulateItems(this.ItemCode);
        }

        [Input]
        public string ItemCode { get; set; }

        [Input]
        public int? Amount { get; set; }

        public override void HandleModel()
        {
            var carts = _DB.Carts;
            carts.Entity.SetValuesFromModel(this);
            carts.Insert();
        }

        public override void MapModel()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.CheckSessionActivity();
            yield return this.FieldRequired(nameof(this.ItemCode));
            yield return this.FieldRequired(nameof(this.Amount));

            var isValid = this.IsValid(
                nameof(this.SessionId),
                nameof(this.SessionKey),
                nameof(this.ItemCode),
                nameof(this.Amount));

            if (isValid)
            {
                this.Library.ValidateItem(this.ItemCode, nameof(ItemCode));
            }
        }
    }
}
