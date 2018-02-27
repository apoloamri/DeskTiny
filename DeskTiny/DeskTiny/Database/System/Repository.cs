using System.Collections.Generic;
using System.Linq;

namespace DeskTiny.Database.System
{
    public class Repository<Entity> : RepositoryBase<Entity> where Entity : class, new()
    {
        public Repository(string tableName) : base(tableName)
        {
            this.Select = new Select<Entity> {
                TableName = tableName,
                Repository = this
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

        public Select<Entity> Select { get; set; }
        
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

            var nonQuery = new NonQuery(
                $"{Operations.INSERT} INTO {this.TableName}({this.NonQueryConditions.ColumnNames}) VALUES({this.NonQueryConditions.ColumnParameters})",
                this.NonQueryConditions.EntityDictionary
                );

            return nonQuery.ExecuteNonQuery(Operations.INSERT);
        }

        public int Update()
        {
            this.NonQueryConditions.CreateColumnParameters(this.Entity);

            var nonQuery = new NonQuery(
                $"{Operations.UPDATE} {this.TableName} SET {this.NonQueryConditions.ColumnValues} {this.GetWhere()}",
                this.NonQueryConditions.EntityDictionary.Union(this.QueryConditions.Parameters).ToDictionary(x => x.Key, x => x.Value)
                );

            return nonQuery.ExecuteNonQuery(Operations.UPDATE);
        }

        public int Delete()
        {
            var nonQuery = new NonQuery(
                $"{Operations.DELETE} FROM {this.TableName} {this.GetWhere()}",
                this.NonQueryConditions.EntityDictionary.Union(this.QueryConditions.Parameters).ToDictionary(x => x.Key, x => x.Value)
                );

            return nonQuery.ExecuteNonQuery(Operations.DELETE);
        }
    }

    public class Select<Entity> where Entity : class, new()
    {
        internal string TableName { get; set; }
        internal Repository<Entity> Repository { get; set; }
        
        public List<Dictionary<string, object>> Dictionaries()
        {
            return this.Repository.SelectBase().GetListDictionary();
        }

        public List<Entity> Entities()
        {
            return this.Repository.SelectBase().GetListEntity<Entity>();
        }
    }
}
