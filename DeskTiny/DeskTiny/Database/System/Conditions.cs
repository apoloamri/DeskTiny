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

        public void LimitBy(int limit)
        {
            this.Limit = limit;
        }

        public void OrderBy(Order order)
        {
            this.Order = order;
        }
        
        private int ColumnCount = 0;
        private string OptionalName = "q_";
        public Order? Order { get; private set; }
        public string Where { get; private set; } = "";
        public List<string> MultiWhere { get; private set; } = new List<string>();
        public Dictionary<string, object> Parameters { get; private set; } = new Dictionary<string, object>();

        public void Which(
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

        public void End(Operator? oper = null)
        {
            this.MultiWhere.Add($"({this.Where}) {oper} ");
            this.Where = string.Empty;
        }
        
        private string GetCondition(Condition condition)
        {
            switch (condition)
            {
                case Condition.EqualTo:
                    return "=";
                case Condition.NotEqualTo:
                    return "!=";
                case Condition.GreaterThan:
                    return ">";
                case Condition.LessThan:
                    return "<";
                case Condition.GreaterThanEqualTo:
                    return ">=";
                case Condition.LessThanEqualTo:
                    return "<=";
                case Condition.Like:
                    return "LIKE";
                case Condition.NotLike:
                    return "NOT LIKE";
                default:
                    return "=";
            }
        }
    }
}
