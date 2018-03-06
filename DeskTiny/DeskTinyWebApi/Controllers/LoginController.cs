using DeskTinyWebApi.Models.Login;
using Microsoft.AspNetCore.Mvc;

namespace DeskTinyWebApi.Controllers
{
    [Route("[controller]")]
    public class LoginController : DeskTiny.Mvc.CustomController
    {
        [HttpGet]
        public JsonResult Get(LoginModel model)
        {
            this.BindModel(ref model);
            this.Validate();

            model.IsLogin();

            this.BuildJson(model);
            return this.JsonResult;
        }

        [HttpPost]
        public JsonResult Post(LoginModel model)
        {
            this.BindModel(ref model);
            this.Validate();

            model.Login();

            this.BuildJson(model);
            return this.JsonResult;
        }
    }
}