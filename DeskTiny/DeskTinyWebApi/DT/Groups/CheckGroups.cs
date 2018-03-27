using DTCore.Database.Enums;
using DTMessenger.DT.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTMessenger.DT.Groups
{
    public static class CheckGroups
    {
        public static bool IsGroupsExisting(string groupName)
        {
            var groups = Schemas.Groups;

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
