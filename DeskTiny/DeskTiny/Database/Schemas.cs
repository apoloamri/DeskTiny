using DTCore.Tools.Extensions;
using DTCore.Database.Tables;
using System.Linq;
using DTCore.Tools;

namespace DTCore.Database
{
    public class Schemas
    {
        public static Schema<T> CreateTable<T>(string tableName, bool updateTable = true) where T : Entity, new()
        {
            if (updateTable == false)
            {
                return new Schema<T>(tableName);
            }

            var table = InformationSchemaTables;

            table.Conditions.AddWhere(
                nameof(table.Entity.table_name),
                Enums.Condition.Equal,
                tableName);

            table.Conditions.AddColumns(
                nameof(table.Entity.table_name));

            var newSchema = new Schema<T>(tableName);

            if (table.Count() == 0)
            {
                newSchema.CreateTable();
            }
            else
            {
                var columns = InformationSchemaColumns;

                columns.Conditions.AddWhere(
                    nameof(table.Entity.table_name),
                    Enums.Condition.Equal,
                    tableName);

                columns.Conditions.AddColumns(
                    nameof(table.Entity.column_name));

                var currentColumns = columns.Select.Entities?.Select(x => x.column_name)?.ToList();
                var entityColumns = newSchema.Entity.GetColumns();

                if (CompareLists.UnorderedEqual(entityColumns, currentColumns))
                {
                    return newSchema;
                }

                newSchema.AddTableColumns(entityColumns.MissingItems(currentColumns));

                currentColumns = columns.Select.Entities?.Select(x => x.column_name)?.ToList();

                newSchema.DropTableColumns(currentColumns.MissingItems(entityColumns));
            }

            return newSchema;
        }

        private static Schema<InformationSchema> InformationSchemaTables => new Schema<InformationSchema>("information_schema.tables");
        private static Schema<InformationSchema> InformationSchemaColumns => new Schema<InformationSchema>("information_schema.columns");
        public static Schema<Accesses> Accesses => CreateTable<Accesses>("accesses");
        public static Schema<Clients> Clients => CreateTable<Clients>("clients");
        public static Schema<Sessions> Sessions => CreateTable<Sessions>("sessions");
    }
}
