﻿using DeskTinyWebApi.Models.Member;
using DTCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DeskTinyWebApi.Controllers
{
    [Route("[controller]")]
    public class MemberController : DTController
    {
        [HttpGet]
        [Route("Information")]
        public JsonResult Information()
        {
            this.Initiate<InformationModel>(true);

            return this.Conclude();
        }

        [HttpPost]
        [Route("Register")]
        public JsonResult Register()
        {
            this.Initiate<RegisterModel>(true);

            return this.Conclude();
        }
    }
}