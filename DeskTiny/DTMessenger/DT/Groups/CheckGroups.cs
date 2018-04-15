﻿using DTCore.Database;

namespace DTMessenger.DT.Groups
{
    public static class CheckGroups
    {
        public static bool IsGroupsExisting(string groupName)
        {
            var groups = Database.Schemas.Groups;

            groups.Conditions.Where(
                groups.Column(x => x.creator),
                Condition.EqualTo,
                groupName);

            groups.Conditions.Where(
                Operator.OR,
                groups.Column(x => x.creator),
                Condition.EqualTo,
                groupName);

            return groups.Count() > 0;
        }
    }
}
