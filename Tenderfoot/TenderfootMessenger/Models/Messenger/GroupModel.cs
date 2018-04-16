using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootMessenger.DT.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TenderfootMessenger.Models.Messenger
{
    public class GroupModel : TfModel
    {
        [Input]
        public int? GroupId { get; set; }

        [Input]
        public int? Count { get; set; }

        [Input]
        public string Message { get; set; }
        
        [JsonProperty]
        public List<Dictionary<string, object>> Result { get; set; }

        public override void HandleModel()
        {
            var messages = DT.Database.Schemas.GroupMessages;
            messages.Entity.group_id = this.GroupId;
            messages.Entity.sender = this.SessionId;
            messages.Entity.message = this.Message;
            messages.Insert();
        }

        public override void MapModel()
        {
            if (!this.GroupId.HasValue)
            {
                this.GetGroups();
            }
            else
            {
                this.GetGroupMessages();
            }
        }

        private void GetGroups()
        {
            var groups = DT.Database.Schemas.Groups;
            groups.Conditions.Where(groups.Column(x => x.members), Condition.EqualTo, this.SessionId);
            this.Result = groups.Select.Dictionaries;
        }

        private void GetGroupMessages()
        {
            var messages = DT.Database.Schemas.GroupMessages;
            messages.Conditions.Where(messages.Column(x => x.group_id), Condition.EqualTo, this.GroupId);
            messages.Conditions.OrderBy(messages.Column(x => x.id), Order.DESC);
            messages.Conditions.LimitBy(this.Count ?? 10);
            this.Result = messages.Select.Dictionaries;
        }
        
        public override IEnumerable<ValidationResult> Validate()
        {
            yield return TfValidationResult.CheckSessionActivity(this.SessionId, this.SessionKey);

            if (this.Handling)
            {
                TfValidationResult.FieldRequired(nameof(this.GroupId), this.GroupId);
                TfValidationResult.FieldRequired(nameof(this.Message), this.Message);
            }
        }
    }
}
