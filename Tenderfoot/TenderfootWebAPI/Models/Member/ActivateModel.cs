using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using TenderfootWebAPI.Library.Member;

namespace TenderfootWebAPI.Models.Member
{
    public class ActivateModel : TfModel<Activate>
    {
        public override void BeforeStartUp()
        {
            this.Library.PopulateMembers(this.Username, this.Key);
        }

        [Input]
        public string Username { get; set; }

        [Input]
        public string Key { get; set; }

        public override void HandleModel()
        {
            this.Library.Members.Entity.active = 1;
            this.Library.Members.Update();
        }

        public override void MapModel()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.Library.CheckKey(nameof(this.Username), nameof(this.Key));
            yield return this.Library.CheckActivity(nameof(this.Username), nameof(this.Key));
        }
    }
}
