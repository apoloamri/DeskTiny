using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;
using TenderfootCampaign.Library.Shop;

namespace TenderfootCampaign.Models.Shop
{
    public class AddCartModel : TfModel<AddCart>
    {
        [Input]
        public string ItemCode { get; set; }

        [Input]
        public int? Amount { get; set; }

        [JsonProperty]
        public List<Dictionary<string, object>> CartItems { get; set; }

        [JsonProperty]
        public int CartTotal { get; set; }

        public override void HandleModel()
        {
            var carts = _DB.Carts;
            carts.Entity.SetValuesFromModel(this);
            carts.Entity.amount = null;
            if (carts.HasRecords)
            {
                carts.SelectToEntity();
                carts.Entity.amount = carts.Entity.amount + this.Amount;
                carts.Update();
            }
            else
            {
                carts.Entity.amount = this.Amount;
                carts.Insert();
            }
        }

        public override void MapModel()
        {
            var carts = _DB.Carts;
            var items = _DB.Items;

            carts.Relate(Join.INNER, items, 
                carts.Relation(carts._("item_code"), items._("item_code")));

            carts.Case.Where(carts._("session_key"), Is.EqualTo, this.SessionKey);

            this.CartItems = carts.Select.Dictionaries;
            this.CartTotal = this.CartItems.Sum(x => Convert.ToInt32(x["price"]) * Convert.ToInt32(x["amount"]));
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.CheckSessionActivity();

            if (this.Handling)
            {
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
}
