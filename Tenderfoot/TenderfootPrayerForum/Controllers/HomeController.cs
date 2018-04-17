﻿using Tenderfoot.Mvc;
using TenderfootPrayerForum.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace TenderfootPrayerForum.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : TfController
    {
        [HttpGet]
        public JsonResult Index()
        {
            this.Initiate<IndexModel>(true);
            return this.Conclude();
        }

        [HttpGet]
        [Route("login")]
        public JsonResult GetLogin()
        {
            this.Initiate<LoginModel>(true);
            return this.Conclude();
        }

        [HttpPost]
        [Route("login")]
        public JsonResult PostLogin()
        {
            this.Initiate<LoginModel>(true);
            return this.Conclude();
        }
    }
}