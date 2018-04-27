using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootCampaign.Models.Campaign;

namespace TenderfootCampaign.Controllers
{
    [Route("api/[controller]")]
    public class CampaignController : TfController
    {
        [HttpGet]
        public JsonResult GetBonus()
        {
            this.Initiate<BonusModel>(false);
            return this.Conclude();
        }

        [HttpPost]
        [Route("validate")]
        public JsonResult PurchaseBonusValidate()
        {
            this.Initiate<BonusModel>(false);
            return this.Validate();
        }

        [HttpPost]
        public JsonResult PurchaseBonus()
        {
            this.Initiate<BonusModel>(false);
            return this.Conclude();
        }
    }
}
