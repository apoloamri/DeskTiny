using Tenderfoot.Database;

namespace TenderfootMessenger.DT.Groups
{
    public static class CheckGroups
    {
        public static bool IsGroupsExisting(string groupName)
        {
            var groups = Database.Schemas.Groups;

            groups.Conditions.Where(
                groups.Column(x => x.creator),
                Is.EqualTo,
                groupName);

            groups.Conditions.Where(
                Operator.OR,
                groups.Column(x => x.creator),
                Is.EqualTo,
                groupName);

            return groups.Count() > 0;
        }
    }
}
