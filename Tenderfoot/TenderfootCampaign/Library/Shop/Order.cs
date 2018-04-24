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
        public Schema<Items> Items { get; set; } = _DB.Items;
        public List<Items> itemList { get; set; }
        public int TotalPrice { get; set; }
        public int TotalItems { get; set; }

        public void PopulateItems(List<OrderItems> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    this.Items.Conditions.Where(Operator.OR, Items.Column(x => x.item_code), Is.EqualTo, item.ItemCode);
                }
                this.itemList = this.Items.Select.Entities;
            }
        }

        public ValidationResult ValidateItems(List<OrderItems> items, params string[] memberNames)
        {
            if (this.itemList.Count() != items.Count())
            {
                return TfValidationResult.Compose("NotExists", memberNames);
            }
            return null;
        }

        public void GeneratePrice()
        {
            this.TotalPrice = this.itemList.Sum(x => x.price) ?? 0;
            this.TotalItems = this.itemList.Count();
        }
    }
    
    public class OrderItems
    {
        [Input]
        public string ItemCode { get; set; }

        [Input]
        public int? ItemCount { get; set; }
    }
}
