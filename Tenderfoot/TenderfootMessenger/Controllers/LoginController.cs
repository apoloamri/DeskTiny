using TenderfootMessenger.Models.Login;
using Tenderfoot.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace TenderfootMessenger.Controllers
{
    [Route("login")]
    public class LoginController : DTController
    {
        [HttpGet]
        public JsonResult Get()
        {
            this.Initiate<LoginModel>(true);
            return this.Conclude();
        }

        [HttpPost]
        public JsonResult Post()
        {
            this.Initiate<LoginModel>(true);
            return this.Conclude();
        }
    }
}