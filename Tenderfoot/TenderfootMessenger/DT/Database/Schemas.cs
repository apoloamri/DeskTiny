using Tenderfoot.Database;

namespace TenderfootMessenger.DT.Database
{
    public class Schemas : Tenderfoot.Database.Schemas
    {
        public static Schema<Tables.Contacts> Contacts => CreateTable<Tables.Contacts>("contacts");
        public static Schema<Tables.Groups> Groups => CreateTable<Tables.Groups>("groups");
        public static Schema<Tables.GroupMessages> GroupMessages => CreateTable<Tables.GroupMessages>("group_messages");
        public static Schema<Tables.Members> Members => CreateTable<Tables.Members>("members");
        public static Schema<Tables.Messages> Messages => CreateTable<Tables.Messages>("messages");
    }
}
