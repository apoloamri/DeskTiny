using System;
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
            this.Library.PopulateItems(this.Items);
        }

        [Input]
        public List<OrderItems> Items { get; set; }
        
        [JsonProperty]
        public int TotalPrice { get; set; }

        [JsonProperty]
        public int TotalItems { get; set; }

        public override void HandleModel()
        {
            throw new NotImplementedException();
        }

        public override void MapModel()
        {
            this.Library.GeneratePrice();
            this.SetValuesFromModel(this.Library);
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValid(nameof(this.Items)))
            {
                yield return this.Library.ValidateItems(this.Items, nameof(this.Items));
            }
        }
    }
}
