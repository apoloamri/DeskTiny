using DeskTiny.Database.Tables;

namespace DeskTiny.Database
{
    public class Schemas
    {
        public static Schema<T> CreateTable<T>(string tableName) where T : Entity, new()
        {
            return new Schema<T>(tableName);
        }

        public static Schema<Accesses> Accesses => CreateTable<Accesses>("accesses");
        public static Schema<Clients> Clients => CreateTable<Clients>("clients");
        public static Schema<Sessions> Sessions => CreateTable<Sessions>("sessions");
    }
}
