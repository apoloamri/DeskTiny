using Tenderfoot.Mvc;
using TenderfootAPI.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace TenderfootAPI.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : DTController
    {
        [HttpGet]
        public JsonResult Index()
        {
            this.Initiate<IndexModel>(true);
            return this.Conclude();
        }
    }
}