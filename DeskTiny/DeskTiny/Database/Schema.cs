using DTCore.Database.System;
using DTCore.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTCore.Database
{
    public class Schema<T> : SchemaBase<T> where T : Entity, new()
    {
        public Schema(string tableName) : base(tableName)
        {
            this.Select = new Select<T> {
                TableName = tableName,
                Schema = this
            };
        }

        public Query SelectBase()
        {
            string columns =
                this.Conditions.Columns?.Count() > 0 ?
                string.Join(", ", this.Conditions.Columns) :
                "*";
            
            string order =
                this.Conditions.Order.HasValue ?
                $"ORDER BY {this.Conditions.Order}" :
                string.Empty;

            string limit =
                this.Conditions.Limit.HasValue ?
                $"LIMIT {this.Conditions.Limit}" :
                string.Empty;

            return new Query(
                $"{Operations.SELECT} {columns} FROM {TableName} {this.GetWhere()} {order} {limit};",
                this.Conditions.Parameters
                );
        }

        public Select<T> Select { get; set; }
        
        public virtual long Count()
        {
            string columns =
                this.Conditions.Columns?.Count() > 0 ?
                string.Join(", ", this.Conditions.Columns) :
                "*";
            
            var query = new Query(
                $"{Operations.SELECT} COUNT({columns}) FROM {TableName} {this.GetWhere()};",
                this.Conditions.Parameters
                );

            return query.GetScalar();
        }

        public int Insert()
        {
            this.NonConditions.CreateColumnParameters(this.Entity);

            if (this.NonConditions.Parameters.Count() == 0)
            {
                return 0;
            }

            if (!this.Entity.insert_time.HasValue)
            {
                this.Entity.insert_time = DateTime.Now;
            }

            var nonQuery = new NonQuery(
                $"{Operations.INSERT} INTO {this.TableName}({this.NonConditions.ColumnNames}) VALUES({this.NonConditions.ColumnParameters});",
                this.NonConditions.Parameters
                );

            return nonQuery.ExecuteNonQuery(Operations.INSERT);
        }

        public int Update()
        {
            this.NonConditions.CreateColumnParameters(this.Entity);

            if (this.NonConditions.Parameters.Count() == 0)
            {
                return 0;
            }

            var nonQuery = new NonQuery(
                $"{Operations.UPDATE} {this.TableName} SET {this.NonConditions.ColumnValues} {this.GetWhere()};",
                this.NonConditions.Parameters.Union(this.Conditions.Parameters).ToDictionary(x => x.Key, x => x.Value)
                );

            return nonQuery.ExecuteNonQuery(Operations.UPDATE);
        }

        public int Delete()
        {
            var nonQuery = new NonQuery(
                $"{Operations.DELETE} FROM {this.TableName} {this.GetWhere()};",
                this.Conditions.Parameters
                );

            return nonQuery.ExecuteNonQuery(Operations.DELETE);
        }

        public int CreateTable()
        {
            var nonQuery = new NonQuery(
                $"{Operations.CREATE_TABLE.GetString()} {this.TableName} ({this.CreateColumns()});",
                this.NonConditions.Parameters
                );

            return nonQuery.ExecuteNonQuery(Operations.CREATE_TABLE);
        }

        public int AddTableColumns(params string[] columns)
        {
            if (columns.Count() == 0)
            {
                return 0;
            }
            
            var nonQuery = new NonQuery(
                $"{Operations.ALTER_TABLE.GetString()} {this.TableName} {this.AddColumns(columns)};",
                this.NonConditions.Parameters
                );
            
            return nonQuery.ExecuteNonQuery(Operations.ALTER_TABLE); ;
        }

        public int DropTableColumns(params string[] columns)
        {
            if (columns.Count() == 0)
            {
                return 0;
            }

            var nonQuery = new NonQuery(
                $"{Operations.ALTER_TABLE.GetString()} {this.TableName} {this.DropColumns(columns)};",
                this.NonConditions.Parameters
                );

            return nonQuery.ExecuteNonQuery(Operations.ALTER_TABLE); ;
        }
    }
    
    public class Select<T> where T : Entity, new()
    {
        internal string TableName { get; set; }
        internal Schema<T> Schema { get; set; }
        public List<Dictionary<string, object>> Dictionaries => this.Schema.SelectBase().GetListDictionary();
        public Dictionary<string, object> Dictionary => this.Schema.SelectBase().GetListDictionary().FirstOrDefault();
        public List<T> Entities => this.Schema.SelectBase().GetListEntity<T>();
        public T Entity => this.Schema.SelectBase().GetListEntity<T>().FirstOrDefault();
    }
}
