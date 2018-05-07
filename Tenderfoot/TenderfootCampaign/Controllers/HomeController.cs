using Tenderfoot.Mvc;
using TenderfootCampaign.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace TenderfootCampaign.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : TfController
    {
        [HttpPost]
        [Route("login/validate")]
        public JsonResult ValidateLogin()
        {
            this.Initiate<LoginModel>();
            return this.Validate();
        }

        [HttpPost]
        [Route("login")]
        public JsonResult Login()
        {
            this.Initiate<LoginModel>();
            return this.Conclude();
        }

        [HttpGet]
        [Route("login")]
        public JsonResult CheckLogin()
        {
            this.Initiate<LoginModel>();
            return this.Validate();
        }
    }
}