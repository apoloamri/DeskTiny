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
    }
}