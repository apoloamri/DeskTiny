using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootEtherWallet.Models.Member;

namespace TenderfootEtherWallet.Controller
{
    [Route("api/member")]
    public class MemberController : TfController
    {
        [HttpGet]
        public JsonResult GetMember()
        {
            this.Initiate<MemberModel>();
            return this.Conclude();
        }

        [HttpPost]
        public JsonResult PostMember()
        {
            this.Initiate<MemberModel>();
            return this.Conclude();
        }

        [HttpPost]
        [Route("login")]
        public JsonResult Login()
        {
            this.Initiate<LoginModel>();
            return this.Conclude();
        }
    }
}
