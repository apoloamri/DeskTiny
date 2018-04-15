using DTCore.DTSystem;
using DTCore.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DTCore.Database.System
{
    public class Conditions<TableEntity> where TableEntity : Entity, new()
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
        
        /// <summary>
        /// Adds an order to be followed when the result is read.
        /// </summary>
        /// <param name="column">The column where the order will depend.</param>
        /// <param name="order">The given order. May it be ascending or descending.</param>
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
        private string Param = ConnectProvider.Param();
        public string Order { get; private set; }
        public string WhereBase { get; private set; } = "";
        public List<string> MultiWhere { get; private set; } = new List<string>();
        public Dictionary<string, object> Parameters { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Sets the criteria for the result when selecting.
        /// </summary>
        /// <param name="column">The column where the criteria will depend.</param>
        /// <param name="condition">The condition of the criteria.</param>
        /// <param name="value">The value to be compared with.</param>
        public void Where(
            TableColumn column,
            Condition condition,
            object value)
        {
            this.Where(Operator.AND, column, condition, value);
        }

        public void Where(
            Operator? oper,
            TableColumn column, 
            Condition condition, 
            object value)
        {
            if (value == null)
            {
                return;
            }
            
            string columnParameter = this.OptionalName + column.Get + this.ColumnCount;
            string statement = string.Empty;


            if (!column.Type.IsArray)
            {
                statement = $"{column.Get} {GetCondition(condition)} {Param}{columnParameter} ";
            }
            else
            {
                statement = $"{Param}{columnParameter} {GetCondition(condition)} ANY({column.Get}) ";
            }
            
            if (this.WhereBase.IsEmpty())
            {
                this.WhereBase += $"{statement} ";
            }
            else
            {
                this.WhereBase += $"{oper ?? Operator.AND} {statement} ";
            }

            var property = typeof(TableEntity).GetProperty(column.ColumnName);

            if (property.GetCustomAttribute<EncryptAttribute>(false) != null &&
                property.PropertyType == typeof(string))
            {
                if (property.GetCustomAttribute<EncryptAttribute>(false) != null)
                {
                    value = Encryption.Encrypt(Convert.ToString(value), true);
                }
            }

            this.Parameters.Add(columnParameter, value);
            this.ColumnCount++;
        }

        public void Exists<T>(Operator? oper, Schema<T> schema, params Relation[] columnOn) where T : Entity, new()
        {
            this.ExistsBase(oper, schema, false, columnOn);
        }

        public void NotExists<T>(Operator? oper, Schema<T> schema, params Relation[] columnOn) where T : Entity, new()
        {
            this.ExistsBase(oper, schema, true, columnOn);
        }

        private void ExistsBase<T>(Operator? oper, Schema<T> schema, bool notExists, params Relation[] columnOn) where T : Entity, new()
        {
            string existence = notExists ?
                "NOT" :
                string.Empty;

            string customName = "sub";

            var onString = new List<string>();

            foreach (var column in columnOn)
            {
                onString.Add($"{column.Column1.GetCustomName(customName)} {GetCondition(column.Condition ?? Condition.EqualTo)} {column.Column2.Get}");
            }
            
            string statement = $"{existence} EXISTS ({Operations.SELECT} 1 FROM {schema.TableName} AS {customName} WHERE {string.Join(", ", onString)})";

            if (this.WhereBase.IsEmpty())
            {
                this.WhereBase += $"{statement} ";
            }
            else
            {
                this.WhereBase += $"{oper ?? Operator.AND} {statement} ";
            }
        }

        /// <summary>
        /// Ends the above criterias which will be set into a group, and sets a group of criterias.
        /// </summary>
        /// <param name="oper">The operator that'll separate the group above and below.</param>
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
