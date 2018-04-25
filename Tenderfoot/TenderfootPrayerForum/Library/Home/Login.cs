﻿using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Library._Database;

namespace TenderfootPrayerForum.Library.Home
{
    public class Login
    {
        public Schema<Members> Members { get; set; } = _DB.Members;

        public void PopulateMembers(string username, string password)
        {
            this.Members.Conditions.Where(this.Members._(x => x.username), Is.EqualTo, username);
            this.Members.Conditions.Where(this.Members._(x => x.password), Is.EqualTo, password);
        }

        public ValidationResult CheckUsernamePassword(params string[] memberNames)
        {
            if (this.Members.Count == 0)
            {
                return TfValidationResult.Compose("InvalidLogin", memberNames);
            }

            return this.CheckActivity(memberNames);
        }

        public ValidationResult CheckActivity(params string[] memberNames)
        {
            this.Members.Conditions.Where(this.Members._(x => x.active), Is.EqualTo, 1);
            return
                this.Members.Count == 0 ?
                TfValidationResult.Compose("InactiveAccount", memberNames) :
                null;
        }
    }
}
