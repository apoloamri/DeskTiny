using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootMessenger.DT.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TenderfootMessenger.Models.Messenger
{
    public class MessagesModel : DTModel
    {
        [Input]
        public string Username { get; set; }

        [Input]
        public string Message { get; set; }

        [Input]
        public string ContactUsername { get; set; }

        [Input]
        public int? Count { get; set; }

        [JsonProperty]
        public List<Dictionary<string, object>> Result { get; set; }

        public override void HandleModel()
        {
            var messages = DT.Database.Schemas.Messages;

            messages.Entity.sender = this.SessionId;
            messages.Entity.recipient = this.Username;
            messages.Entity.message = this.Message;
            messages.Entity.unread = 1;

            messages.Insert();
        }

        public override void MapModel()
        {
            this.GetMessages();
            this.ReadMessages();
            
        }

        private void ReadMessages()
        {
            var messages = DT.Database.Schemas.Messages;
            var members = DT.Database.Schemas.Members;

            messages.Relate(Join.LEFT, members,
                messages.Relation(messages.Column(x => x.sender), members.Column(x => x.username)));

            messages.Conditions.Where(
                messages.Column(x => x.sender),
                Condition.EqualTo,
                this.Username);

            messages.Conditions.Where(
                messages.Column(x => x.recipient),
                Condition.EqualTo,
                this.SessionId);

            messages.Conditions.OrderBy(messages.Column(x => x.insert_time), Order.DESC);
            messages.Conditions.LimitBy(this.Count ?? 10);

            messages.Entity.unread = 0;

            messages.Update();
        }

        private void GetMessages()
        {
            var messages = DT.Database.Schemas.Messages;
            var members = DT.Database.Schemas.Members;

            messages.Relate(Join.LEFT, members,
                messages.Relation(messages.Column(x => x.sender), members.Column(x => x.username)));

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

            messages.Conditions.OrderBy(messages.Column(x => x.insert_time), Order.DESC);
            messages.Conditions.LimitBy(this.Count ?? 10);
            
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
