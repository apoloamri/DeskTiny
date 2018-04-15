using DTCore.Mvc;
using DTTemplateAPI.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace DTTemplateAPI.Controllers
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