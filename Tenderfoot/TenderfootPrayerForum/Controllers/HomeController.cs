using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Models.Home;

namespace TenderfootPrayerForum.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : TfController
    {
        [HttpGet]
        public JsonResult Get()
        {
            this.Initiate<HomeModel>();
            return this.Conclude();
        }
    }
}
