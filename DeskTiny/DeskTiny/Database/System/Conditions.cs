using DTCore.Database.Enums;
using DTCore.Tools.Extensions;
using System.Collections.Generic;

namespace DTCore.Database.System
{
    public class Conditions
    {
        public string[] Columns { get; private set; }

        public void AddColumns(params string[] columns)
        {
            this.Columns = columns;
        }

        public int? Limit { get; private set; }

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
        public Order? Order { get; private set; }
        public string Where { get; private set; } = "";
        public List<string> MultiWhere { get; private set; } = new List<string>();
        public Dictionary<string, object> Parameters { get; private set; } = new Dictionary<string, object>();

        public void AddWhere(
            object column, 
            Condition condition, 
            object value, 
            Operator? oper = null)
        {
            if (value == null)
            {
                return;
            }
            
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
                    return Condition.LIKE.GetString();
                case Condition.NOT_LIKE:
                    return Condition.NOT_LIKE.GetString();
                default:
                    return "=";
            }
        }
    }
}
