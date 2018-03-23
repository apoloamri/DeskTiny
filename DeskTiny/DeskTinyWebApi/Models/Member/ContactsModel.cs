using DeskTinyWebApi.DT.Database;
using DTCore.Database.Enums;
using DTCore.Mvc;
using DTCore.Mvc.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeskTinyWebApi.Models.Member
{
    public class ContactsModel : DTModel
    {
        [JsonProperty]
        public List<Dictionary<string, object>> Result { get; set; }

        [JsonProperty]
        public List<Dictionary<string, object>> UnacceptedResults { get; set; }

        public override void HandleModel()
        {
            throw new NotImplementedException();
        }

        public override void MapModel()
        {
            var contacts = Schemas.Contacts;
            var members = Schemas.Members;

            contacts.Relate(Join.INNER, members,
                contacts.Relation(contacts.Column(x => x.contact_username), members.Column(x => x.username)));

            contacts.Conditions.Where(
                contacts.Column(x => x.username),
                Condition.EqualTo,
                this.SessionId);

            this.Result = contacts.Select.Dictionaries;

            contacts.ClearRelation();
            contacts.ClearConditions();

            contacts.Relate(Join.INNER, members,
                contacts.Relation(contacts.Column(x => x.username), members.Column(x => x.username)));

            contacts.Conditions.Where(
                contacts.Column(x => x.contact_username),
                Condition.EqualTo,
                this.SessionId);

            contacts.Conditions.NotExists(Operator.AND, contacts,
                contacts.Relation(contacts.Column(x => x.contact_username), contacts.Column(x => x.username)));
            
            this.UnacceptedResults = contacts.Select.Dictionaries;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return DTValidationResult.CheckSessionActivity(this.SessionId, this.SessionKey);
        }
    }
}
