﻿using Tenderfoot.Database;
using Tenderfoot.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TenderfootMessenger.Models.Member
{
    public class ContactsModel : TfModel
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
            this.GetContacts();
            this.GetRequests();
        }

        private void GetRequests()
        {
            var contacts = DT.Database.Schemas.Contacts;
            var members = DT.Database.Schemas.Members;

            contacts.Relate(Join.INNER, members,
                contacts.Relation(contacts.Column(x => x.username), members.Column(x => x.username)));

            contacts.Conditions.Where(
                contacts.Column(x => x.contact_username),
                Is.EqualTo,
                this.SessionId);

            contacts.Conditions.NotExists(Operator.AND, contacts,
                contacts.Relation(contacts.Column(x => x.contact_username), contacts.Column(x => x.username)));

            this.UnacceptedResults = contacts.Select.Dictionaries;
        }

        private void GetContacts()
        {
            var contacts = DT.Database.Schemas.Contacts;
            var members = DT.Database.Schemas.Members;

            contacts.Relate(Join.INNER, members,
                contacts.Relation(contacts.Column(x => x.contact_username), members.Column(x => x.username)));

            contacts.Conditions.Where(
                contacts.Column(x => x.username),
                Is.EqualTo,
                this.SessionId);

            this.Result = new List<Dictionary<string, object>>();

            foreach (var contact in contacts.Select.Dictionaries)
            {
                var messages = DT.Database.Schemas.Messages;

                messages.Conditions.Where(
                    messages.Column(x => x.recipient),
                    Is.EqualTo,
                    this.SessionId);

                messages.Conditions.Where(
                    messages.Column(x => x.sender),
                    Is.EqualTo,
                    contact["username1"]);

                messages.Conditions.Where(
                    messages.Column(x => x.unread),
                    Is.EqualTo,
                    1);
                
                contact["message_count"] = messages.Count();

                this.Result.Add(contact);
            }
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return TfValidationResult.CheckSessionActivity(this.SessionId, this.SessionKey);
        }
    }
}
