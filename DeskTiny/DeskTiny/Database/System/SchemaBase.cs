using System.Linq;

namespace DeskTiny.Database.System
{
    public class SchemaBase<T> where T : Entity, new()
    {
        protected string TableName { get; set; }

        protected string GetWhere()
        {
            string where = string.Empty;

            if (this.QueryConditions.MultiWhere?.Count() > 0)
            {
                where += string.Join(" ", this.QueryConditions.MultiWhere);
            }

            where +=
                !string.IsNullOrEmpty(this.QueryConditions.Where) ?
                this.QueryConditions.Where :
                string.Empty;

            return 
                !string.IsNullOrEmpty(where) ?
                $"WHERE {where}" :
                string.Empty;
        }

        protected SchemaBase(string tableName) { this.TableName = tableName; }

        public QueryConditions QueryConditions { get; set; } = new QueryConditions();

        public void ClearQueryConditions() { this.QueryConditions = new QueryConditions(); }
        
        protected NonQueryConditions NonQueryConditions { get; set; } = new NonQueryConditions();

        public void ClearNonQueryConditions() { this.NonQueryConditions = new NonQueryConditions(); }

        public T Entity { get; set; } = new T();
        
        public void ClearEntity() { this.Entity = new T(); }
    }

    public enum Operations { SELECT, INSERT, UPDATE, DELETE }
}
