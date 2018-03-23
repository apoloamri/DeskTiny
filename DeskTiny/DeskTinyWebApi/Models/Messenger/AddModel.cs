﻿using DeskTinyWebApi.DT.Database;
using DeskTinyWebApi.DT.Messenger;
using DTCore.Mvc;
using DTCore.Mvc.Attributes;
using DTCore.WebApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeskTinyWebApi.Models.Messenger
{
    public class AddModel : DTModel
    {
        [Input]
        public string Username { get; set; }
        
        public override void HandleModel()
        {
            var contacts = Schemas.Contacts;
            contacts.Entity.username = this.SessionId;
            contacts.Entity.contact_username = this.Username;
            contacts.Insert();
        }

        public override void MapModel()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return DTValidationResult.CheckSessionActivity(this.SessionId, this.SessionKey);
            yield return DTValidationResult.FieldRequired(nameof(this.Username), this.Username);

            if (CheckAdd.CheckContactExists(this.SessionId, this.Username))
            {
                yield return DTValidationResult.Compose("ContactExists", nameof(this.Username));
            }
        }
    }
}
