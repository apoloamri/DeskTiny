using DTCore.Mvc;
using DTStorybook.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace DTStorybook.Controllers
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