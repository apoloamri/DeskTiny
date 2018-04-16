using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Models.Member;

namespace TenderfootPrayerForum.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : DTController
    {
        [HttpPost]
        [Route("register")]
        public JsonResult Register()
        {
            this.Initiate<RegisterModel>(true);
            return this.Conclude();
        }
    }
}