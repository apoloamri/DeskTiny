using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Models.Member;

namespace TenderfootPrayerForum.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : TfController
    {
        [HttpPost]
        [Route("register")]
        public JsonResult Register()
        {
            this.Initiate<RegisterModel>(true);
            return this.Conclude();
        }

        [HttpPut]
        [Route("activate")]
        public JsonResult Activate()
        {
            this.Initiate<ActivateModel>(true);
            return this.Conclude();
        }
    }
}