using System.Collections.Generic;

namespace DeskTiny.Database.System
{
    public class QueryConditions
    {
        internal string[] Columns { get; set; }

        public void AddColumns(params string[] columns)
        {
            this.Columns = columns;
        }

        internal int? Limit { get; set; }

        public void AddLimit(int limit)
        {
            this.Limit = limit;
        }

        public void AddOrder(Order order)
        {
            this.Order = order;
        }
        
        private int ColumnCount = 0;
        private string OptionalName = "q_";
        internal Order? Order { get; set; }
        internal string Where { get; set; } = "";
        internal List<string> MultiWhere { get; set; } = new List<string>();
        internal Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        public void AddWhere(
            string column, 
            Condition condition, 
            object value, 
            Operator? oper = null)
        {
            string columnParameter = this.OptionalName + column + this.ColumnCount;
            string statement = $"{column} {this.GetCondition(condition)} :{columnParameter} ";

            if (string.IsNullOrEmpty(this.Where))
            {
                this.Where += $"{statement} ";
            }
            else
            {
                this.Where += $"{oper ?? Operator.AND} {statement} ";
            }
            
            this.Parameters.Add(columnParameter, value);
            this.ColumnCount++;
        }

        public void EndWhere(Operator? oper = null)
        {
            this.MultiWhere.Add($"({this.Where}) {oper} ");
            this.Where = string.Empty;
        }
        
        private string GetCondition(Condition condition)
        {
            switch (condition)
            {
                case Condition.Equal:
                    return "=";
                case Condition.NotEqual:
                    return "!=";
                case Condition.Greater:
                    return ">";
                case Condition.Lesser:
                    return "<";
                case Condition.GreaterEqual:
                    return ">=";
                case Condition.LesserEqual:
                    return "<=";
                case Condition.LIKE:
                    return "LIKE";
                default:
                    return "=";
            }
        }
    }
}
