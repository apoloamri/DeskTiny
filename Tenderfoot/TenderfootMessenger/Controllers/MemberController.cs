using TenderfootMessenger.Models.Member;
using Tenderfoot.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace TenderfootMessenger.Controllers
{
    [Route("member")]
    public class MemberController : DTController
    {
        [HttpGet]
        [Route("contacts")]
        public JsonResult Contacts()
        {
            this.Initiate<ContactsModel>(true);
            return this.Conclude();
        }
        
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