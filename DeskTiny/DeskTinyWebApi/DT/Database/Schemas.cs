using DTCore.Database;

namespace DeskTinyWebApi.DT.Database
{
    public class Schemas : DTCore.Database.Schemas
    {
        public static Schema<Tables.Members> Members => CreateTable<Tables.Members>("members");
        public static Schema<Tables.Contacts> Contacts => CreateTable<Tables.Contacts>("contacts");
    }
}
