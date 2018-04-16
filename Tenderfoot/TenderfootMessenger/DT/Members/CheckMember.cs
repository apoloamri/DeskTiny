using Tenderfoot.Database;
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
                Condition.EqualTo,
                email);

            return member.Count() > 0;
        }

        public static bool CheckUsernameExists(string username)
        {
            var member = Database.Schemas.Members;

            member.Conditions.Where(
                member.Column(x => x.username),
                Condition.EqualTo,
                username);

            return member.Count() > 0;
        }

        public static bool CheckUsernamePasswordExists(string username, string password)
        {
            var member = Database.Schemas.Members;

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
