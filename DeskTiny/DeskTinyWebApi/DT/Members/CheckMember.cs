using DeskTinyWebApi.DT.Database;
using DTCore.Database.Enums;

namespace DeskTinyWebApi.DT.Members
{
    public static class CheckMember
    {
        public static bool CheckEmailExists(string email)
        {
            var member = Schemas.Members;

            member.Conditions.Where(
                member.Column(x => x.email),
                Condition.EqualTo,
                email);

            return member.Count() > 0;
        }

        public static bool CheckUsernameExists(string username)
        {
            var member = Schemas.Members;

            member.Conditions.Where(
                member.Column(x => x.username),
                Condition.EqualTo,
                username);

            return member.Count() > 0;
        }

        public static bool CheckUsernamePasswordExists(string username, string password)
        {
            var member = Schemas.Members;

            member.Conditions.Where(
                member.Column(x => x.username),
                Condition.EqualTo,
                username);

            member.Conditions.Where(
                member.Column(x => x.password),
                Condition.EqualTo,
                password);

            return member.Count() > 0;
        }
    }
}
