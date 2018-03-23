using DeskTinyWebApi.DT.Database;
using DTCore.Database.Enums;
using DTCore.Mvc;
using DTCore.Mvc.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeskTinyWebApi.Models.Messenger
{
    public class MessagesModel : DTModel
    {
        [Input]
        public string Username { get; set; }

        [Input]
        public string Message { get; set; }

        [Input]
        public string ContactUsername { get; set; }

        [JsonProperty]
        public List<Dictionary<string, object>> Result { get; set; }

        public override void HandleModel()
        {
            var messages = Schemas.Messages;

            messages.Entity.sender = this.SessionId;
            messages.Entity.recipient = this.Username;
            messages.Entity.message = this.Message;

            messages.Insert();
        }

        public override void MapModel()
        {
            var messages = Schemas.Messages;
            var members = Schemas.Members;

            messages.Relate(Join.LEFT, members,
                messages.Relation(messages.Column(x => x.recipient), members.Column(x => x.username)));

            messages.Conditions.Where(
                messages.Column(x => x.sender),
                Condition.EqualTo,
                this.SessionId);

            messages.Conditions.Where(
                messages.Column(x => x.recipient),
                Condition.EqualTo,
                this.Username);

            messages.Conditions.End(Operator.OR);

            messages.Conditions.Where(
                messages.Column(x => x.sender),
                Condition.EqualTo,
                this.Username);

            messages.Conditions.Where(
                messages.Column(x => x.recipient),
                Condition.EqualTo,
                this.SessionId);

            this.Result = messages.Select.Dictionaries;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return DTValidationResult.CheckSessionActivity(this.SessionId, this.SessionKey);
            yield return DTValidationResult.FieldRequired(nameof(this.Username), this.Username);

            if (this.Handling)
            {
                yield return DTValidationResult.FieldRequired(nameof(this.Message), this.Message);
            }
        }
    }
}
