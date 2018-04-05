using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTMessenger.Models.Messenger;
using DTCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DTMessenger.Controllers
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