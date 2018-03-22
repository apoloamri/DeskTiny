using DeskTinyWebApi.Models.Login;
using DTCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DeskTinyWebApi.Controllers
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