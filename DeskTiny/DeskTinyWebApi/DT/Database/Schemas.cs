using DTCore.Database;

namespace DTMessenger.DT.Database
{
    public class Schemas : DTCore.Database.Schemas
    {
        public static Schema<Tables.Contacts> Contacts => CreateTable<Tables.Contacts>("contacts");
        public static Schema<Tables.Members> Members => CreateTable<Tables.Members>("members");
        public static Schema<Tables.Messages> Messages => CreateTable<Tables.Messages>("messages");
    }
}
