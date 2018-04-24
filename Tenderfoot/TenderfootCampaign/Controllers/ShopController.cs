﻿using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootCampaign.Models.Shop;

namespace TenderfootCampaign.Controllers
{
    [Route("api/[controller]")]
    public class ShopController : TfController
    {
        [HttpGet]
        [Route("items")]
        public JsonResult Items()
        {
            this.Initiate<ItemsModel>();
            return this.Conclude();
        }

        [HttpGet]
        [Route("order")]
        public JsonResult GetOrder()
        {
            this.Initiate<OrderModel>();
            return this.Conclude();
        }

        [HttpPost]
        [Route("order")]
        public JsonResult PostOrder()
        {
            this.Initiate<OrderModel>();
            return this.Conclude();
        }
        
        [HttpPost]
        [Route("cart")]
        public JsonResult AddCart()
        {
            this.Initiate<AddCartModel>();
            return this.Conclude();
        }
    }
}
