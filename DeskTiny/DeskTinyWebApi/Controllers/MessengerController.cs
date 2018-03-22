using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeskTinyWebApi.Models.Messenger;
using DTCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DeskTinyWebApi.Controllers
{
    [Route("messenger")]
    public class MessengerController : DTController
    {
        [HttpGet]
        [Route("search")]
        public JsonResult Search()
        {
            this.Initiate<SearchModel>(true);
            return this.Conclude();
        }
    }
}