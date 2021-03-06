﻿using Tenderfoot.Database;
using TenderfootMessenger.DT.Database;

namespace TenderfootMessenger.DT.Members
{
    public static class CheckMember
    {
        public static bool CheckEmailExists(string email)
        {
            var member = Database.Schemas.Members;

            member.Conditions.Where(
                member.Column(x => x.email),
                Is.EqualTo,
                email);

            return member.Count() > 0;
        }

        public static bool CheckUsernameExists(string username)
        {
            var member = Database.Schemas.Members;

            member.Conditions.Where(
                member.Column(x => x.username),
                Is.EqualTo,
                username);

            return member.Count() > 0;
        }

        public static bool CheckUsernamePasswordExists(string username, string password)
        {
            var member = Database.Schemas.Members;

            member.Conditions.Where(
                member.Column(x => x.username),
                Is.EqualTo,
                username);

            member.Conditions.Where(
                member.Column(x => x.password),
                Is.EqualTo,
                password);

            return member.Count() > 0;
        }
    }
}
