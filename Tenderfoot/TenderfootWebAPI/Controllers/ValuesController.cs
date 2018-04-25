using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootBoxCampaign.Daabase;

namespace TenderfootBoxCampaign.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : TfController
    {
        [HttpGet]
        public JsonResult Get()
        {
            this.Initiate<IndexModel>(false);
            return this.Conclude();
        }

        [HttpPost]
        public JsonResult Post()
        {
            this.Initiate<IndexModel>(false);
            return this.Conclude();
        }

        [HttpPost]
        [Route("validate")]
        public JsonResult PostValidate()
        {
            this.Initiate<IndexModel>(false);
            return this.Validate();
        }
    }

    internal class IndexModel : TfModel
    {
        [JsonProperty]
        public List<Dictionary<string, object>> MemberList { get; set; }

        public override void HandleModel()
        {
            throw new NotImplementedException();
        }

        public override void MapModel()
        {
            var members = _DB.Members;
            this.MemberList = members.Select.Dictionaries;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }
    }
}
