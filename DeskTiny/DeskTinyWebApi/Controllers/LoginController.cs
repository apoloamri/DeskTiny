using DTMessenger.Models.Login;
using DTCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DTMessenger.Controllers
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