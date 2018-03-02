using DeskTinyWebApi.Models.Login;
using Microsoft.AspNetCore.Mvc;

namespace DeskTinyWebApi.Controllers
{
    [Route("[controller]")]
    public class LoginController : DeskTiny.Mvc.CustomController
    {
        [HttpPost]
        public JsonResult Login(LoginModel model)
        {
            model = this.GetJson<LoginModel>();
            
            return this.Json(model);
        }
    }
}