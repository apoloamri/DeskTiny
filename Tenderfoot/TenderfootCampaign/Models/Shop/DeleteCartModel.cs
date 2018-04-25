using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;
using TenderfootCampaign.Library.Shop;

namespace TenderfootCampaign.Models.Shop
{
    public class DeleteCartModel : TfModel<AddCart>
    {
        [Input]
        public string ItemCode { get; set; }

        public override void HandleModel()
        {
            var carts = _DB.Carts;
            carts.Entity.SetValuesFromModel(this);
            if (carts.HasRecords)
            {
                carts.Delete();
            }
        }

        public override void MapModel()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.CheckSessionActivity();

            if (this.Handling)
            {
                yield return this.FieldRequired(nameof(this.ItemCode));

                var isValid = this.IsValid(
                    nameof(this.SessionId),
                    nameof(this.SessionKey),
                    nameof(this.ItemCode));

                if (isValid)
                {
                    this.Library.ValidateItem(this.ItemCode, nameof(ItemCode));
                }
            }
        }
    }
}
