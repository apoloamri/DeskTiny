using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Library.Shop
{
    public class AddCart
    {
        public Schema<Items> Items { get; set; } = _DB.Items;

        public void PopulateItems(string itemCode)
        {
            this.Items.Conditions.Where(Operator.OR, this.Items.Column(x => x.item_code), Is.EqualTo, itemCode);
        }

        public ValidationResult ValidateItem(string itemCode, params string[] memberNames)
        {
            if (!this.Items.HasRecords())
            {
                return TfValidationResult.Compose("NotExists", memberNames);
            }
            return null;
        }
    }
}
