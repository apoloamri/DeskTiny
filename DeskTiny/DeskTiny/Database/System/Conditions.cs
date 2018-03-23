using DTCore.Database.Enums;
using DTCore.Tools.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace DTCore.Database.System
{
    public class Conditions
    {
        public string[] Columns { get; private set; }

        public void AddColumns(params TableColumn[] columns)
        {
            this.Columns = columns.Select(x => x.Get)?.ToArray();
        }

        public int? Limit { get; private set; }

        public void LimitBy(int limit)
        {
            this.Limit = limit;
        }
        
        public void OrderBy(TableColumn column, Order order)
        {
            if (this.Order.IsEmpty())
            {
                this.Order = $"{column.Get} {order.GetString()}";
            }
            else
            {
                this.Order = $", {column.Get} {order.GetString()}";
            }
        }
        
        private int ColumnCount = 0;
        private string OptionalName = "q_";
        public string Order { get; private set; }
        public string WhereBase { get; private set; } = "";
        public List<string> MultiWhere { get; private set; } = new List<string>();
        public Dictionary<string, object> Parameters { get; private set; } = new Dictionary<string, object>();

        public void Where(
            TableColumn column, 
            Condition condition, 
            object value, 
            Operator? oper = null)
        {
            if (value == null)
            {
                return;
            }
            
            string columnParameter = this.OptionalName + column.Get + this.ColumnCount;
            string statement = $"{column.Get} {GetCondition(condition)} :{columnParameter} ";

            if (this.WhereBase.IsEmpty())
            {
                this.WhereBase += $"{statement} ";
            }
            else
            {
                this.WhereBase += $"{oper ?? Operator.AND} {statement} ";
            }
            
            this.Parameters.Add(columnParameter, value);
            this.ColumnCount++;
        }

        public void End(Operator? oper = null)
        {
            this.MultiWhere.Add($"({this.WhereBase}) {oper} ");
            this.WhereBase = string.Empty;
        }
        
        public static string GetCondition(Condition condition)
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
