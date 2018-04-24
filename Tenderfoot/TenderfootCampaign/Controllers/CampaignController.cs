using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootCampaign.Models.Campaign;

namespace TenderfootCampaign.Controllers
{
    public class CampaignController : TfController
    {
        [HttpGet]
        public JsonResult Index()
        {
            this.Initiate<IndexModel>(false);
            return this.Conclude();
        }
    }
}
