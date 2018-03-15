using DeskTinyWebApi.Models.Login;
using Microsoft.AspNetCore.Mvc;

namespace DeskTinyWebApi.Controllers
{
    [Route("[controller]")]
    public class LoginController : DTCore.Mvc.DTController
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