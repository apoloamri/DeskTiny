using DeskTiny.Mvc;
using Microsoft.AspNetCore.Mvc;
using TestDekTinyWebApi.Model;

namespace TestDekTinyWebApi.Controllers
{
    [Route("api/client")]
    public class ClientController : CustomController
    {
        [HttpGet]
        public JsonResult GetClients(ClientModel model)
        {
            model.GetRegisteredClients();

            return this.Json(model);
        }

        [HttpPost]
        [Route("register")]
        public JsonResult RegisterClient(ClientModel model)
        {
            model = this.GetJson<ClientModel>();

            model.RegisterClient();

            return this.Json(model);
        }

        [HttpPut]
        [Route("update")]
        public JsonResult UpdateClient(ClientModel model)
        {
            model = this.GetJson<ClientModel>();

            model.UpdateClient();

            return this.Json(model);
        }
    }
}