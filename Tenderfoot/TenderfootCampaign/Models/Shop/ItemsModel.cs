using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Models.Shop
{
    public class ItemsModel : TfModel
    {
        [Input]
        [ValidateInput(InputType.String)]
        public string ProductName { get; set; }

        [Input]
        [ValidateInput(InputType.Number)]
        public int? ProductType { get; set; }

        [JsonProperty]
        public List<Dictionary<string, object>> Result { get; set; }

        public override void HandleModel()
        {
            throw new NotImplementedException();
        }

        public override void MapModel()
        {
            var items = _DB.Items;
            if (this.IsValid(nameof(this.ProductName)))
            {
                items.Case.Where(items._(x => x.name), Is.Like, $" %{this.ProductName}%");
            }
            if (this.IsValid(nameof(this.ProductType)))
            {
                items.Case.Where(items._(x => x.product_type), Is.EqualTo, this.ProductType);
            }
            this.Result = items.Select.Dictionaries;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }
    }
}
