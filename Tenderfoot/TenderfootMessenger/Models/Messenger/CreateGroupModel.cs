using Tenderfoot.Mvc;
using TenderfootMessenger.DT.Database;
using TenderfootMessenger.DT.Members;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TenderfootMessenger.Models.Messenger
{
    public class CreateGroupModel : TfModel
    {
        [Input]
        public string GroupName { get; set; }

        [Input]
        public List<string> Members { get; set; }
        
        public override void HandleModel()
        {
            var groups = Schemas.Groups;

            this.Members.Add(this.SessionId);

            groups.Entity.name = this.GroupName;
            groups.Entity.creator = this.SessionId;
            groups.Entity.members = this.Members.ToArray();

            groups.Insert();
        }

        public override void MapModel()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return TfValidationResult.CheckSessionActivity(this.SessionId, this.SessionKey);

            if (this.Handling)
            {
                yield return TfValidationResult.FieldRequired(nameof(this.GroupName), this.GroupName);
                yield return TfValidationResult.FieldRequired(nameof(this.Members), this.Members);

                if (this.Members != null && this.Members.Count > 0)
                {
                    foreach (var member in this.Members)
                    {
                        if (!CheckMember.CheckUsernameExists(member))
                        {
                            yield return TfValidationResult.Compose("UsernameNotExists", new[] { member }, nameof(this.Members));
                        }
                    }
                }
            }
        }
    }
}
