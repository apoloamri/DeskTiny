using DTMessenger.DT.Database;
using DTCore.Database.Enums;
using DTCore.Tools.Extensions;

namespace DTMessenger.DT.Messenger
{
    public static class CheckAdd
    {
        public static bool CheckContactExists(string username, string contactUsername)
        {
            if (username.IsEmpty() && contactUsername.IsEmpty())
            {
                return false;
            }

            var contacts = Schemas.Contacts;

            contacts.Conditions.Where(
                contacts.Column(x => x.username),
                Condition.EqualTo,
                username);

            contacts.Conditions.Where(
                contacts.Column(x => x.contact_username),
                Condition.EqualTo,
                contactUsername);
            
            return contacts.Count() > 0;
        }
    }
}
