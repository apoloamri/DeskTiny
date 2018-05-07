using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Models.Request;

namespace TenderfootPrayerForum.Controllers
{
    [Route("api/[controller]")]
    public class RequestController : TfController
    {
        #region INSERT REQUEST
        [HttpPost]
        [Route("validate")]
        public JsonResult PostRequestValidate()
        {
            this.Initiate<RequestModel>();
            return this.Validate();
        }

        [HttpPost]
        public JsonResult PostRequest()
        {
            this.Initiate<RequestModel>();
            return this.Conclude();
        }
        #endregion

        #region GET REQUEST
        [HttpGet]
        [Route("validate")]
        public JsonResult GetRequestValidate()
        {
            this.Initiate<GetRequestModel>();
            return this.Validate();
        }

        [HttpGet]
        public JsonResult GetRequest()
        {
            this.Initiate<GetRequestModel>();
            return this.Conclude();
        }
        #endregion

        #region ACTIVATE REQUEST
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
    }
}
