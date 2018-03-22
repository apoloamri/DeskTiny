using DeskTinyWebApi.DT.Database;
using DTCore.Database.Enums;
using DTCore.Mvc;
using DTCore.Mvc.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeskTinyWebApi.Models.Messenger
{
    public class SearchModel : DTModel
    {
        [Input]
        public string Email { get; set; }

        [Input]
        public string Username { get; set; }

        [JsonProperty]
        public List<Dictionary<string, object>> Result { get; set; }

        public override void HandleModel()
        {
            throw new NotImplementedException();
        }

        public override void MapModel()
        {
            var members = Schemas.Members;

            members.Conditions.Where(
                members.Column(x => x.username),
                Condition.Like,
                $"%{this.Username}%");

            members.Conditions.Where(
                members.Column(x => x.email),
                Condition.Like,
                $"%{this.Email}%",
                Operator.OR);

            this.Result = members.Select.Dictionaries;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (string.IsNullOrEmpty(this.Email) && string.IsNullOrEmpty(this.Username))
            {
                yield return DTValidationResult.FieldRequired(nameof(this.Email), this.Email);
                yield return DTValidationResult.FieldRequired(nameof(this.Username), this.Username);
            }
        }
    }
}
