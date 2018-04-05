using DTCore.Database.Attributes;
using DTCore.Database.Enums;
using DTCore.Database.System;
using DTCore.DTSystem;
using DTCore.Tools;
using DTCore.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DTCore.Database
{
    public class Schema<T> : SchemaBase<T> where T : Entity, new()
    {
        public Schema(string tableName) : base(tableName)
        {
            this.Select = new Select<T> {
                TableName = tableName,
                Schema = this
            };
        }

        public void Relate<Joined>(Join join, Schema<Joined> schema, params Relation[] columnOn) where Joined : Entity, new()
        {
            var onString = new List<string>();
            
            foreach (var column in columnOn)
            {
                onString.Add($"{column.Column1.Get} {Conditions<Joined>.GetCondition(column.Condition ?? Condition.EqualTo)} {column.Column2.Get}");
            }
            
            this.Join.Add(new JoinItem() { Join = join, TableName = schema.TableName, OnString = string.Join(", ", onString) });
        }

        public Query SelectBase()
        {
            string columns =
                this.Conditions.Columns?.Count() > 0 ?
                string.Join(", ", this.Conditions.Columns) :
                "*";

            string join =
                this.Join?.Count() > 0 ?
                string.Join(" ", this.Join.Select(x => { return $"{x.Join.GetString()} JOIN {x.TableName} ON {x.OnString}"; })) :
                string.Empty;

            string order =
                !this.Conditions.Order.IsEmpty() ?
                $"ORDER BY {this.Conditions.Order}" :
                string.Empty;

            string limit =
                this.Conditions.Limit.HasValue ?
                $"LIMIT {this.Conditions.Limit}" :
                string.Empty;

            foreach (var property in typeof(T).GetProperties())
            {
                if (property.GetCustomAttribute<EncryptAttribute>(false) != null &&
                    this.Conditions.Parameters.ContainsKey(property.Name))
                {
                    this.Conditions.Parameters[property.Name] = Encryption.Decrypt(Convert.ToString(this.Conditions.Parameters[property.Name]), true);
                }
            }

            return new Query(
                $"{Operations.SELECT} {columns} FROM {TableName} {join} {this.GetWhere()} {order} {limit};",
                this.Conditions.Parameters
                );
        }

        public Select<T> Select { get; set; }
        
        public virtual long Count()
        {
            string columns =
                this.Conditions.Columns?.Count() > 0 ?
                string.Join(", ", this.Conditions.Columns) :
                "*";
            
            var query = new Query(
                $"{Operations.SELECT} COUNT({columns}) FROM {TableName} {this.GetWhere()};",
                this.Conditions.Parameters
                );

            return query.GetScalar();
        }

        public int Insert()
        {
            this.NonConditions.CreateColumnParameters(this.Entity);

            if (this.NonConditions.Parameters.Count() == 0)
            {
                return 0;
            }

            if (!this.Entity.insert_time.HasValue)
            {
                this.Entity.insert_time = DateTime.Now;
            }

            var nonQuery = new NonQuery(
                $"{Operations.INSERT} INTO {this.TableName}({this.NonConditions.ColumnNames}) VALUES({this.NonConditions.ColumnParameters});",
                this.NonConditions.Parameters
                );

            return nonQuery.ExecuteNonQuery(Operations.INSERT);
        }

        public int Update()
        {
            this.NonConditions.CreateColumnParameters(this.Entity);

            if (this.NonConditions.Parameters.Count() == 0)
            {
                return 0;
            }
            
            var nonQuery = new NonQuery(
                $"{Operations.UPDATE} {this.TableName} SET {this.NonConditions.ColumnValues} {this.GetWhere(this.Join)};",
                this.NonConditions.Parameters.Union(this.Conditions.Parameters).ToDictionary(x => x.Key, x => x.Value)
                );

            return nonQuery.ExecuteNonQuery(Operations.UPDATE);
        }

        public int Delete()
        {
            var nonQuery = new NonQuery(
                $"{Operations.DELETE} FROM {this.TableName} {this.GetWhere(this.Join)};",
                this.Conditions.Parameters
                );

            return nonQuery.ExecuteNonQuery(Operations.DELETE);
        }

        public int CreateTable()
        {
            var nonQuery = new NonQuery(
                $"{Operations.CREATE_TABLE.GetString()} {this.TableName} ({this.CreateColumns()});",
                this.NonConditions.Parameters
                );
            
            return nonQuery.ExecuteNonQuery(Operations.CREATE_TABLE);
        }

        public int AddTableColumns(params string[] columns)
        {
            if (columns.Count() == 0)
            {
                return 0;
            }
            
            var nonQuery = new NonQuery(
                $"{Operations.ALTER_TABLE.GetString()} {this.TableName} {this.AddColumns(columns)};",
                this.NonConditions.Parameters
                );
            
            return nonQuery.ExecuteNonQuery(Operations.ALTER_TABLE); ;
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public int DropTableColumns(params string[] columns)
        {
            if (columns.Count() == 0)
            {
                return 0;
            }

            var nonQuery = new NonQuery(
                $"{Operations.ALTER_TABLE.GetString()} {this.TableName} {this.DropColumns(columns)};",
                this.NonConditions.Parameters
                );

            return nonQuery.ExecuteNonQuery(Operations.ALTER_TABLE); ;
        }
    }
    
    public class Select<T> where T : Entity, new()
    {
        internal string TableName { get; set; }
        internal Schema<T> Schema { get; set; }

        public List<Dictionary<string, object>> Dictionaries
        {
            get
            {
                var dictionary = this.Schema.SelectBase().GetListDictionary();

                foreach (var item in dictionary)
                {
                    foreach (var property in typeof(T).GetProperties())
                    {
                        if (property.GetCustomAttribute<EncryptAttribute>(false) != null &&
                            item.ContainsKey(property.Name))
                        {
                            item[property.Name] = Encryption.Decrypt(Convert.ToString(item[property.Name]), true);
                        }
                    }
                }

                return dictionary;
            }
        }

        public Dictionary<string, object> Dictionary => this.Schema.SelectBase().GetListDictionary().FirstOrDefault();

        public List<T> Entities
        {
            get
            {
                return this.Dictionaries.Select(item => {
                    return DictionaryClassConverter.DictionaryToClass<T>(item);
                })?.ToList();
            }
        }

        public T Entity => this.Entities.FirstOrDefault();
    }
}
