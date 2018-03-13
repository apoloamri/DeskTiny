using DeskTinyWebApi.DT.Database.Tables;

namespace DeskTinyWebApi.DT.Database
{
    public class Schemas : DTCore.Database.Schemas
    {
        public static DTCore.Database.Schema<Members> Members => CreateTable<Members>("members");
    }
}
