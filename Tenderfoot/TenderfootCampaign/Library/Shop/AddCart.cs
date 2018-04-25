using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Library.Shop
{
    public class AddCart
    {
        public ValidationResult ValidateItem(string itemCode, params string[] memberNames)
        {
            var items = _DB.Items;
            items.Case.Where(items._("item_code"), Is.EqualTo, itemCode);
            if (!items.HasRecord)
            {
                return TfValidationResult.Compose("NotExists", memberNames);
            }
            return null;
        }
    }
}
