using DTCore.Database.Attributes;
using DTCore.Database.Enums;
using DTCore.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DTCore.Database.System
{
    public class SchemaBase<T> where T : Entity, new()
    {
        public string TableName { get; set; }

        protected SchemaBase(string tableName) { this.TableName = tableName; }

        protected List<JoinItem> Join { get; set; } = new List<JoinItem>();
        
        protected NonConditions NonConditions { get; set; } = new NonConditions();

        protected void ClearNonConditions() { this.NonConditions = new NonConditions(); }

        protected string GetWhere(List<JoinItem> join = null)
        {
            string where = string.Empty;
            string joinedTables = string.Empty;

            if (join != null && join.Count() > 0)
            {
                if (this.Join.Count() > 0)
                {
                    joinedTables = $"FROM {string.Join(", ", this.Join.Select(x => x.TableName))}";
                    where += $"{string.Join(", ", this.Join.Select(x => x.OnString))} AND ";
                }
            }

            if (this.Conditions.MultiWhere?.Count() > 0)
            {
                where += string.Join(" ", this.Conditions.MultiWhere);
            }

            where +=
                !this.Conditions.WhereBase.IsEmpty() ?
                this.Conditions.WhereBase :
                string.Empty;

            return 
                !where.IsEmpty() ?
                $"{joinedTables} WHERE {where}" :
                string.Empty;
        }
        
        protected string CreateColumns()
        {
            var columnList = new List<string>();

            var type = this.Entity.GetType();

            foreach (var property in type.GetProperties())
            {
                string column = this.CreateColumn(property);

                if (column.IsEmpty())
                {
                    continue;
                }
                 
                columnList.Add(column);
            }

            return string.Join(",", columnList);
        }
        
        protected string AddColumns(params string[] columns)
        {
            var columnList = new List<string>();

            foreach (var column in columns)
            {
                var property = this.Entity.GetType().GetProperty(column);
                var newColumn = this.CreateColumn(property);

                if (newColumn.IsEmpty())
                {
                    continue;
                }

                columnList.Add($"{Operations.ADD} {newColumn}");
            }

            return string.Join(",", columnList); ;
        }

        protected string DropColumns(params string[] columns)
        {
            var columnList = new List<string>();

            foreach (var column in columns)
            {
                string alteredColumn = $"{Operations.DROP_COLUMN.GetString()} {column}";
                columnList.Add(alteredColumn);
            }

            return string.Join(",", columnList); ;
        }
        
        private string CreateColumn(PropertyInfo property)
        {
            var customAttributes = property.GetCustomAttributes(false);

            string column = property.Name;
            string dataType = this.GetColumnDataType(property.PropertyType, customAttributes);
            string attributes = string.Empty;
            
            if (dataType.IsEmpty())
            {
                return string.Empty;
            }

            if (customAttributes != null)
            {
                attributes += $" {this.GetColumnAttributes(customAttributes)}";
            }

            if (attributes.Contains(DataType.SERIAL.GetString()))
            {
                dataType = string.Empty;
            }

            string array = property.PropertyType.IsArray ? "[]" : string.Empty;

            return $"{column} {dataType} {attributes} {array}";
        }

        private string GetColumnDataType(Type type, object[] attributes)
        {
            bool hasSerial = false;
            bool hasText = false;

            foreach (var attribute in attributes)
            {
                if (attribute is Serial)
                {
                    hasSerial = true;
                }

                if (attribute is Text)
                {
                    hasText = true;
                }

                if (attribute is NonTableColumn)
                {
                    return string.Empty;
                }
            }
            
            if (type == typeof(string) ||
                type == typeof(string[]))
            {
                return
                    !hasText ?
                    DataType.CHARACTER_VARYING.GetString() :
                    DataType.TEXT.GetString();
            }
            else if (
                type == typeof(int) ||
                type == typeof(int?) ||
                type == typeof(int[]) ||
                type == typeof(int?[]))
            {
                return
                    !hasSerial ?
                    DataType.INTEGER.GetString() :
                    DataType.SERIAL.GetString();
            }
            else if (
                type == typeof(long) ||
                type == typeof(long?) ||
                type == typeof(long[]) ||
                type == typeof(long?[]))
            {
                return
                    !hasSerial ?
                    DataType.BIGINT.GetString() :
                    DataType.BIGSERIAL.GetString();
            }
            else if (
                type == typeof(short) ||
                type == typeof(short?) ||
                type == typeof(bool) ||
                type == typeof(bool?) ||
                type == typeof(short[]) ||
                type == typeof(short?[]) ||
                type == typeof(bool[]) ||
                type == typeof(bool?[]))
            {
                return
                    !hasSerial ?
                    DataType.SMALLINT.GetString() :
                    DataType.SMALLSERIAL.GetString();
            }
            else if (
                type == typeof(DateTime) ||
                type == typeof(DateTime?) ||
                type == typeof(DateTime[]) ||
                type == typeof(DateTime?[]))
            {
                return DataType.TIMESTAMP_WITHOUT_TIME_ZONE.GetString();
            }

            return DataType.CHARACTER_VARYING.GetString();
        }

        private string GetColumnAttributes(object[] attributes)
        {
            var attributeString = string.Empty; 

            foreach (var attribute in attributes)
            {
                if (attribute is Length)
                {
                    var attr = attribute as Length;
                    attributeString = $"({attr.LengthCount}) ";
                }

                if (attribute is NotNull)
                {
                    var attr = attribute as NotNull;
                    attributeString += $"{ColumnAttributes.NOT_NULL.GetString()} ";
                }

                if (attribute is Default)
                {
                    var attr = attribute as Default;

                    attributeString += $"DEFAULT ";

                    if (attr.DefaultFunction.HasValue)
                    {
                        switch (attr.DefaultFunction)
                        {
                            case Functions.Now:
                                attributeString += $"Now()";
                                break;
                        }
                    }
                    else if (!attr.DefaultObject.IsEmpty())
                    {
                        attributeString += $"'{attr.DefaultObject}'";
                    }
                }

                if (attribute is PrimaryKey)
                {
                    attributeString += $"{ColumnAttributes.PRIMARY_KEY.GetString()} ";
                }

                if (attribute is Unique)
                {
                    attributeString += $"{ColumnAttributes.UNIQUE} ";
                }
            }

            return attributeString;
        }

        public void ClearRelation() { this.Join = new List<JoinItem>(); }

        public Conditions Conditions { get; set; } = new Conditions();

        public void ClearConditions() { this.Conditions = new Conditions(); }

        public T Entity { get; set; } = new T();

        public void ClearEntity() { this.Entity = new T(); }

        public TableColumn Column<TProp>(Expression<Func<T, TProp>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                return null;
            }

            return new TableColumn()
            {
                ColumnName = body.Member.Name,
                TableName = this.TableName
            };
        }

        public Relation Relation(TableColumn column1, TableColumn column2, Condition? condition = null)
        {
            return new Relation()
            {
                Column1 = column1,
                Column2 = column2,
                Condition = condition
            };
        }
    }

    public enum DataType { BIGINT, BIGSERIAL, CHARACTER_VARYING, INTEGER, SERIAL, SMALLINT, SMALLSERIAL, TIMESTAMP_WITHOUT_TIME_ZONE, TEXT }
    public enum Operations { SELECT, INSERT, UPDATE, DELETE, CREATE_TABLE, ALTER_TABLE, ADD, DROP_COLUMN }
    public enum ColumnAttributes { NOT_NULL, PRIMARY_KEY, UNIQUE }

    public class TableColumn
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string Get => $"{this.TableName}.{this.ColumnName}";
        public string GetCustomName(string customName)
        {
            return $"{customName}.{ this.ColumnName}";
        }
    }

    public class Relation
    {
        public TableColumn Column1 { get; set; }
        public TableColumn Column2 { get; set; }
        public Condition? Condition { get; set; }
    }

    public class JoinItem
    {
        public Join? Join { get; set; }
        public string TableName { get; set; }
        public string OnString { get; set; }
    }
}
