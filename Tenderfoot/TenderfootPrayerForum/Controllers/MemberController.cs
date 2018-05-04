using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Models.Member;

namespace TenderfootPrayerForum.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : TfController
    {
        [HttpGet]
        [Route("login")]
        public JsonResult GetLogin()
        {
            this.Initiate<LoginModel>();
            return this.Validate();
        }

        [HttpPost]
        [Route("login/validate")]
        public JsonResult LoginValidate()
        {
            this.Initiate<LoginModel>();
            return this.Validate();
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
