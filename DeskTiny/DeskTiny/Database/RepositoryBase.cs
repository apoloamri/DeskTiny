namespace DeskTiny.Database
{
    public class RepositoryBase<T> where T : class, new()
    {
        protected string TableName { get; set; }
        
        public RepositoryBase(string tableName) { this.TableName = tableName; }

        public QueryConditions QueryConditions { get; set; } = new QueryConditions();

        public void ClearQueryConditions() { this.QueryConditions = new QueryConditions(); }
        
        public NonQueryConditions NonQueryConditions { get; set; } = new NonQueryConditions();

        public void ClearNonQueryConditions() { this.NonQueryConditions = new NonQueryConditions(); }

        public T Entity { get; set; } = new T();
        
        public void ClearEntity() { this.Entity = new T(); }
    }

    public enum Operations { SELECT, INSERT, UPDATE, DELETE }
}
