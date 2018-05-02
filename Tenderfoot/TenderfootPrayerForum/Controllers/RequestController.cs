using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Models.Request;

namespace TenderfootPrayerForum.Controllers
{
    [Route("api/[controller]")]
    public class RequestController : TfController
    {
        [HttpPost]
        public JsonResult PostRequest()
        {
            this.Initiate<RequestModel>();
            return this.Conclude();
        }
    }
}
