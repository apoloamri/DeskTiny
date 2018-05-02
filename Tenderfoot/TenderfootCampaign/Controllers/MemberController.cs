using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootCampaign.Models.Member;

namespace TenderfootCampaign.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : TfController
    {
        [HttpPost]
        [Route("register/validate")]
        public JsonResult RegisterValidate()
        {
            this.Initiate<RegisterModel>();
            return this.Validate();
        }

        [HttpPost]
        [Route("register")]
        public JsonResult Register()
        {
            this.Initiate<RegisterModel>();
            return this.Conclude();
        }

        [HttpGet]
        [Route("points")]
        public JsonResult GetPoints()
        {
            this.Initiate<PointsModel>();
            return this.Conclude();
        }

        [HttpGet]
        [Route("wallet")]
        public JsonResult GetWallet()
        {
            this.Initiate<WalletModel>();
            return this.Conclude();
        }

        [HttpPut]
        [Route("wallet")]
        public JsonResult PutWallet()
        {
            this.Initiate<WalletModel>();
            return this.Conclude();
        }
    }
}