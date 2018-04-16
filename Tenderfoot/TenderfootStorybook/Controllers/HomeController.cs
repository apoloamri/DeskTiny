using Tenderfoot.Mvc;
using TenderfootStorybook.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace TenderfootStorybook.Controllers
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