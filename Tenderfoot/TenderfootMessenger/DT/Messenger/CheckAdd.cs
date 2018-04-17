using Tenderfoot.Database;
using Tenderfoot.Tools.Extensions;

namespace TenderfootMessenger.DT.Messenger
{
    public static class CheckAdd
    {
        public static bool CheckContactExists(string username, string contactUsername)
        {
            if (username.IsEmpty() && contactUsername.IsEmpty())
            {
                return false;
            }

            var contacts = Database.Schemas.Contacts;

            contacts.Conditions.Where(
                contacts.Column(x => x.username),
                Is.EqualTo,
                username);

            contacts.Conditions.Where(
                contacts.Column(x => x.contact_username),
                Is.EqualTo,
                contactUsername);
            
            return contacts.Count() > 0;
        }
    }
}
