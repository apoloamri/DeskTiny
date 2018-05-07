using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [Route("order/validate")]
        public JsonResult PostOrderValidate()
        {
            this.Initiate<OrderModel>();
            return this.Validate();
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

        [HttpGet]
        [Route("cart")]
        public JsonResult GetCart()
        {
            this.Initiate<AddCartModel>();
            return this.Conclude();
        }

        [HttpDelete]
        [Route("cart")]
        public JsonResult DeleteCart()
        {
            this.Initiate<DeleteCartModel>();
            return this.Conclude();
        }
    }
}
