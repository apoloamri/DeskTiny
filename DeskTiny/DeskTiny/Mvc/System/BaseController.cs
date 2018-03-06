using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DeskTiny.Mvc.System
{
    public class BaseController : Controller
    {
        internal JsonSerializerSettings jsonSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
        public JsonResult JsonResult { get; set; }
    }
}
