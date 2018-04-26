using Tenderfoot.Database.Tables;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;
using System.Linq;
using System.Threading;

namespace Tenderfoot.Database
{
    public class Schemas
    {
        /// <summary>
        /// Creates the schema table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">Table name for the schema to be created.</param>
        /// <returns></returns>
        public static Schema<T> CreateTable<T>(string tableName) where T : Entity, new()
        {
            if (TfSettings.Database.Migrate == false)
            {
                return new Schema<T>(tableName);
            }

            var table = InformationSchemaTables;
            table.Case.Where(table._(x => x.table_name), Is.EqualTo, tableName);
            table.Case.AddColumns(table._(x => x.table_name));

            var newSchema = new Schema<T>(tableName);

            if (table.Count == 0)
            {
                newSchema.CreateTable();
            }
            else
            {
                var columns = InformationSchemaColumns;

                columns.Case.Where(columns._(x => x.table_name), Is.EqualTo, tableName);
                columns.Case.AddColumns(columns._(x => x.column_name));

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
        public static Schema<Accesses> Accesses => CreateTable<Accesses>("accesses");
        public static Schema<Emails> Emails => CreateTable<Emails>("emails");
        public static Schema<Sessions> Sessions => CreateTable<Sessions>("sessions");
        public static Schema<SetupChanges> SetupChanges => CreateTable<SetupChanges>("setup_changes");
    }
}
