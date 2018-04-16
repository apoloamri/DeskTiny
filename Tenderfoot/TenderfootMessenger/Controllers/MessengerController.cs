using Tenderfoot.Mvc;
using TenderfootMessenger.Models.Messenger;
using Microsoft.AspNetCore.Mvc;

namespace TenderfootMessenger.Controllers
{
    [Route("messenger")]
    public class MessengerController : DTController
    {
        [HttpPost]
        [Route("add")]
        public JsonResult Add()
        {
            this.Initiate<AddModel>(true);
            return this.Conclude();
        }

        [HttpGet]
        [Route("group")]
        public JsonResult GetGroups()
        {
            this.Initiate<GroupModel>(true);
            return this.Conclude();
        }

        [HttpPost]
        [Route("group")]
        public JsonResult SendGroups()
        {
            this.Initiate<GroupModel>(true);
            return this.Conclude();
        }

        [HttpPost]
        [Route("group/create")]
        public JsonResult CreateGroup()
        {
            this.Initiate<CreateGroupModel>(true);
            return this.Conclude();
        }

        [HttpGet]
        [Route("message")]
        public JsonResult GetMessages()
        {
            this.Initiate<MessagesModel>(true);
            return this.Conclude();
        }

        [HttpPost]
        [Route("message")]
        public JsonResult SendMessages()
        {
            this.Initiate<MessagesModel>(true);
            return this.Conclude();
        }

        [HttpGet]
        [Route("search")]
        public JsonResult Search()
        {
            this.Initiate<SearchModel>(true);
            return this.Conclude();
        }
    }
}