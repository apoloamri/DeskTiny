using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeskTiny.Mvc;
using TestDekTinyWebApi.Model;

namespace TestDekTinyWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DefaultController : CustomController
    {
        // GET api/values
        [HttpGet]
        public JsonResult Get(DefaultModel model)
        {


            return this.Json(model);
        }
        
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}