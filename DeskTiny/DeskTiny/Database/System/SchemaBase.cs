using System.Linq;

namespace DeskTiny.Database.System
{
    public class SchemaBase<T> where T : Entity, new()
    {
        protected string TableName { get; set; }

        protected string GetWhere()
        {
            string where = string.Empty;

            if (this.Conditions.MultiWhere?.Count() > 0)
            {
                where += string.Join(" ", this.Conditions.MultiWhere);
            }

            where +=
                !string.IsNullOrEmpty(this.Conditions.Where) ?
                this.Conditions.Where :
                string.Empty;

            return 
                !string.IsNullOrEmpty(where) ?
                $"WHERE {where}" :
                string.Empty;
        }

        protected SchemaBase(string tableName) { this.TableName = tableName; }

        public Conditions Conditions { get; set; } = new Conditions();

        public void ClearConditions() { this.Conditions = new Conditions(); }
        
        protected NonConditions NonConditions { get; set; } = new NonConditions();

        protected void ClearNonConditions() { this.NonConditions = new NonConditions(); }

        public T Entity { get; set; } = new T();
        
        public void ClearEntity() { this.Entity = new T(); }
    }

    public enum Operations { SELECT, INSERT, UPDATE, DELETE }
}
