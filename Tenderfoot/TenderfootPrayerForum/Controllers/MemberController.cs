using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Models.Member;

namespace TenderfootPrayerForum.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : TfController
    {
        #region LOGIN
        [HttpGet]
        [Route("login")]
        public JsonResult GetLogin()
        {
            this.Initiate<LoginModel>();
            return this.Validate();
        }

        [HttpPost]
        [Route("login/validate")]
        public JsonResult LoginValidate()
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
        #endregion

        #region MEMBER REGISTER
        [HttpPost]
        [Route("register/validate")]
        public JsonResult RegisterValidate()
        {
            this.Initiate<RegisterModel>();
            return this.Validate();
        }

        [HttpPost]
        [Route("register")]
        public JsonResult Register()
        {
            this.Initiate<RegisterModel>();
            return this.Conclude();
        }
        #endregion

        #region MEMBER ACTIVATION
        [HttpPost]
        [Route("activate/validate")]
        public JsonResult ActivateValidate()
        {
            this.Initiate<ActivateModel>();
            return this.Validate();
        }

        [HttpPost]
        [Route("activate")]
        public JsonResult Activate()
        {
            this.Initiate<ActivateModel>();
            return this.Conclude();
        }
        #endregion

        #region MEMBER INFORMATION
        [HttpGet]
        public JsonResult GetMember()
        {
            this.Initiate<MemberModel>();
            return this.Conclude();
        }
        #endregion
    }
}
