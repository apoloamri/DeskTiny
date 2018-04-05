using DTCore.Database.Enums;
using DTCore.Database.Tables;
using DTCore.DTSystem;
using DTCore.Tools;
using DTCore.Tools.Extensions;
using System.Linq;
using System.Threading;

namespace DTCore.Database
{
    public class Schemas
    {
        public static Schema<T> CreateTable<T>(string tableName) where T : Entity, new()
        {
            if (ConfigurationBuilder.Database.Migrate == false)
            {
                return new Schema<T>(tableName);
            }

            var table = InformationSchemaTables;

            table.Conditions.Where(
                table.Column(x => x.table_name),
                Condition.EqualTo,
                tableName);

            table.Conditions.AddColumns(
                table.Column(x => x.table_name));

            var newSchema = new Schema<T>(tableName);

            if (table.Count() == 0)
            {
                newSchema.CreateTable();
            }
            else
            {
                var columns = InformationSchemaColumns;

                columns.Conditions.Where(
                    columns.Column(x => x.table_name),
                    Condition.EqualTo,
                    tableName);

                columns.Conditions.AddColumns(
                    columns.Column(x => x.column_name));

                var currentColumns = columns.Select.Entities?.Select(x => x.column_name)?.ToList();
                var entityColumns = newSchema.Entity.GetColumns();

                if (CompareLists.UnorderedEqual(entityColumns, currentColumns))
                {
                    return newSchema;
                }

                newSchema.AddTableColumns(entityColumns.MissingItems(currentColumns));

                Thread.Sleep(1000);

                currentColumns = columns.Select.Entities?.Select(x => x.column_name)?.ToList();

                newSchema.DropTableColumns(currentColumns.MissingItems(entityColumns));
            }

            return newSchema;
        }
        
        private static Schema<InformationSchema> InformationSchemaTables => new Schema<InformationSchema>("information_schema.tables");
        private static Schema<InformationSchema> InformationSchemaColumns => new Schema<InformationSchema>("information_schema.columns");
        public static Schema<Sessions> Sessions => CreateTable<Sessions>("sessions");
        public static Schema<SetupChanges> SetupChanges => CreateTable<SetupChanges>("setup_changes");
    }
}
