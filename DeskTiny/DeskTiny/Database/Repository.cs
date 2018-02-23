using System.Collections.Generic;
using System.Linq;

namespace DeskTiny.Database
{
    public class Repository<Entity> : RepositoryBase<Entity> where Entity : class, new()
    {
        public Repository(string tableName) : base(tableName) { }

        public virtual List<Entity> Select()
        {
            string columns =
                this.QueryConditions.Columns?.Count() > 0 ?
                string.Join(", ", this.QueryConditions.Columns) :
                "*";

            string where = $"WHERE ";

            if (this.QueryConditions.MultiWhere?.Count() > 0)
            {
                where += string.Join(" ", this.QueryConditions.MultiWhere);
            }

            where +=
                !string.IsNullOrEmpty(this.QueryConditions.Where) ?
                this.QueryConditions.Where :
                string.Empty;

            string order =
                this.QueryConditions.Order.HasValue ?
                $"ORDER BY {this.QueryConditions.Order}" :
                string.Empty;

            string limit =
                this.QueryConditions.Limit.HasValue ?
                $"LIMIT {this.QueryConditions.Limit}" :
                string.Empty;

            var query = new Query(
                $"{Operations.SELECT} {columns} FROM {TableName} {where} {order} {limit};",
                this.QueryConditions.Parameters
                );

            return query.GetListEntity<Entity>();
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
    }
}
