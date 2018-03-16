using DTCore.Database.Attributes;
using DTCore.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DTCore.Database.System
{
    public class SchemaBase<T> where T : Entity, new()
    {
        protected string TableName { get; set; }

        protected SchemaBase(string tableName) { this.TableName = tableName; }
        
        protected NonConditions NonConditions { get; set; } = new NonConditions();

        protected void ClearNonConditions()
        {
            this.NonConditions = new NonConditions();
        }

        protected string GetWhere()
        {
            string where = string.Empty;

            if (this.Wherein.MultiWhere?.Count() > 0)
            {
                where += string.Join(" ", this.Wherein.MultiWhere);
            }

            where +=
                !string.IsNullOrEmpty(this.Wherein.Where) ?
                this.Wherein.Where :
                string.Empty;

            return 
                !string.IsNullOrEmpty(where) ?
                $"WHERE {where}" :
                string.Empty;
        }
        
        protected string CreateColumns()
        {
            var columnList = new List<string>();

            var type = this.Entity.GetType();

            foreach (var property in type.GetProperties())
            {
                columnList.Add(this.CreateColumn(property));
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

                if (string.IsNullOrEmpty(newColumn))
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
            
            if (string.IsNullOrEmpty(dataType))
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

            return $"{column} {dataType} {attributes}";
        }

        private string GetColumnDataType(Type type, object[] attributes)
        {
            bool hasSerial = false;

            foreach (var attribute in attributes)
            {
                if (attribute is Serial)
                {
                    hasSerial = true;
                }

                if (attribute is NonTableColumn)
                {
                    return string.Empty;
                }
            }
            
            if (type == typeof(string))
            {
                return DataType.CHARACTER_VARYING.GetString();
            }
            else if (
                type == typeof(int) ||
                type == typeof(int?))
            {
                return 
                    !hasSerial ? 
                    DataType.INTEGER.GetString() :
                    DataType.SERIAL.GetString();
            }
            else if (
                type == typeof(long) ||
                type == typeof(long?))
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
                type == typeof(bool?))
            {
                return
                    !hasSerial ?
                    DataType.SMALLINT.GetString() :
                    DataType.SMALLSERIAL.GetString();
            }
            else if (
                type == typeof(DateTime) ||
                type == typeof(DateTime?))
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
                            case DefaultFunctions.Now:
                                attributeString += $"Now()";
                                break;
                        }
                    }
                    else if (attr.DefaultObject != null)
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
        
        public Conditions Wherein { get; set; } = new Conditions();

        public void ClearConditions() { this.Wherein = new Conditions(); }

        public T Entity { get; set; } = new T();
        
        public void ClearEntity() { this.Entity = new T(); }
    }

    public enum DataType { BIGINT, BIGSERIAL, CHARACTER_VARYING, INTEGER, SERIAL, SMALLINT, SMALLSERIAL, TIMESTAMP_WITHOUT_TIME_ZONE }
    public enum Operations { SELECT, INSERT, UPDATE, DELETE, CREATE_TABLE, ALTER_TABLE, ADD, DROP_COLUMN }
    public enum ColumnAttributes { NOT_NULL, PRIMARY_KEY, UNIQUE }
}
