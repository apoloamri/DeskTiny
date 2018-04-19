using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootWebAPI.Models.Member;

namespace TenderfootWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : TfController
    {
        [HttpPost]
        [Route("register")]
        public JsonResult Register()
        {
            this.Initiate<RegisterModel>();
            return this.Conclude();
        }

        [HttpPut]
        [Route("activate")]
        public JsonResult Activate()
        {
            this.Initiate<ActivateModel>();
            return this.Conclude();
        }
    }
}