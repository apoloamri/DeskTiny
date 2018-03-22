using DeskTinyWebApi.Models.Member;
using DTCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DeskTinyWebApi.Controllers
{
    [Route("member")]
    public class MemberController : DTController
    {
        [HttpGet]
        [Route("info")]
        public JsonResult Information()
        {
            this.Initiate<InformationModel>(true);
            return this.Conclude();
        }

        [HttpPost]
        [Route("register")]
        public JsonResult Register()
        {
            this.Initiate<RegisterModel>(true);
            return this.Conclude();
        }
    }
}