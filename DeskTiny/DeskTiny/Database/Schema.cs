using DeskTiny.Database.System;
using System.Collections.Generic;
using System.Linq;

namespace DeskTiny.Database
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

        internal Query SelectBase()
        {
            string columns =
                this.QueryConditions.Columns?.Count() > 0 ?
                string.Join(", ", this.QueryConditions.Columns) :
                "*";
            
            string order =
                this.QueryConditions.Order.HasValue ?
                $"ORDER BY {this.QueryConditions.Order}" :
                string.Empty;

            string limit =
                this.QueryConditions.Limit.HasValue ?
                $"LIMIT {this.QueryConditions.Limit}" :
                string.Empty;

            return new Query(
                $"{Operations.SELECT} {columns} FROM {TableName} {this.GetWhere()} {order} {limit};",
                this.QueryConditions.Parameters
                );
        }

        public Select<T> Select { get; set; }
        
        public virtual long Count()
        {
            string columns =
                this.QueryConditions.Columns?.Count() > 0 ?
                string.Join(", ", this.QueryConditions.Columns) :
                "*";
            
            var query = new Query(
                $"{Operations.SELECT} COUNT({columns}) FROM {TableName} {this.GetWhere()};",
                this.QueryConditions.Parameters
                );

            return query.GetScalar();
        }

        public int Insert()
        {
            this.NonQueryConditions.CreateColumnParameters(this.Entity);

            if (this.NonQueryConditions.Parameters.Count() == 0)
            {
                return 0;
            }

            var nonQuery = new NonQuery(
                $"{Operations.INSERT} INTO {this.TableName}({this.NonQueryConditions.ColumnNames}) VALUES({this.NonQueryConditions.ColumnParameters})",
                this.NonQueryConditions.Parameters
                );

            return nonQuery.ExecuteNonQuery(Operations.INSERT);
        }

        public int Update()
        {
            this.NonQueryConditions.CreateColumnParameters(this.Entity);

            if (this.NonQueryConditions.Parameters.Count() == 0)
            {
                return 0;
            }

            var nonQuery = new NonQuery(
                $"{Operations.UPDATE} {this.TableName} SET {this.NonQueryConditions.ColumnValues} {this.GetWhere()}",
                this.NonQueryConditions.Parameters.Union(this.QueryConditions.Parameters).ToDictionary(x => x.Key, x => x.Value)
                );

            return nonQuery.ExecuteNonQuery(Operations.UPDATE);
        }

        public int Delete()
        {
            var nonQuery = new NonQuery(
                $"{Operations.DELETE} FROM {this.TableName} {this.GetWhere()}",
                this.QueryConditions.Parameters
                );

            return nonQuery.ExecuteNonQuery(Operations.DELETE);
        }
    }
    
    public class Select<T> where T : Entity, new()
    {
        internal string TableName { get; set; }
        internal Schema<T> Schema { get; set; }
        public List<Dictionary<string, object>> Dictionaries => this.Schema.SelectBase().GetListDictionary();
        public List<T> Entities => this.Schema.SelectBase().GetListEntity<T>();
    }
}
