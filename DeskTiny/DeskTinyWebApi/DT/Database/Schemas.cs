using DeskTinyWebApi.DT.Database.Tables;

namespace DeskTinyWebApi.DT.Database
{
    public class Schemas : DeskTiny.Database.Schemas
    {
        public static DeskTiny.Database.Schema<Members> Members => CreateTable<Members>("accesses");
    }
}
