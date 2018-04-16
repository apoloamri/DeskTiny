using Tenderfoot.Database.System;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tenderfoot.Database
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

        /// <summary>
        /// Creates a relationship between two schemas.
        /// </summary>
        /// <typeparam name="Joined"></typeparam>
        /// <param name="join">The type of relationship. May it be INNER, OUTER, LEFT or RIGHT.</param>
        /// <param name="schema">The schema to be related with.</param>
        /// <param name="columnOn">The columns will be used for relating the two schemas provided.</param>
        public void Relate<Joined>(Join join, Schema<Joined> schema, params Relation[] columnOn) where Joined : Entity, new()
        {
            var onString = new List<string>();
            
            foreach (var column in columnOn)
            {
                onString.Add($"{column.Column1.Get} {Conditions<Joined>.GetCondition(column.Condition ?? Condition.EqualTo)} {column.Column2.Get}");
            }
            
            var joinItem = new JoinItem()
            {
                Join = join,
                TableName = schema.TableName,
                OnString = string.Join(", ", onString),
                EntityType = typeof(Joined)
            };

            this.Join.Add(joinItem);
            this.Select.Joined.Add(joinItem);
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

        /// <summary>
        /// Selects the condition result either by entity or dictionary.
        /// </summary>
        public Select<T> Select { get; set; }
        
        /// <summary>
        /// Counts the number of result by the provided condition.
        /// </summary>
        /// <returns>The count result.</returns>
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

        /// <summary>
        /// Inserts the entity's values into the schema.
        /// </summary>
        /// <returns>The count of the inserted entities.</returns>
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

            return this.ExecuteNonQuery(
                $"{Operations.INSERT} INTO {this.TableName}({this.NonConditions.ColumnNames}) VALUES({this.NonConditions.ColumnParameters});",
                this.NonConditions.Parameters,
                Operations.INSERT
                );
        }

        /// <summary>
        /// Updates selected entities from the schema.
        /// </summary>
        /// <returns>The count of the updated schemas.</returns>
        public int Update()
        {
            this.NonConditions.CreateColumnParameters(this.Entity);

            if (this.NonConditions.Parameters.Count() == 0)
            {
                return 0;
            }

            return this.ExecuteNonQuery(
                $"{Operations.UPDATE} {this.TableName} SET {this.NonConditions.ColumnValues} {this.GetWhere(this.Join)};",
                this.NonConditions.Parameters.Union(this.Conditions.Parameters).ToDictionary(x => x.Key, x => x.Value),
                Operations.UPDATE
                );
        }

        /// <summary>
        /// Deletes the selected entities from the schema.
        /// </summary>
        /// <returns>The count of the deleted schemas.</returns>
        public int Delete()
        {
            return this.ExecuteNonQuery(
                $"{Operations.DELETE} FROM {this.TableName} {this.GetWhere(this.Join)};",
                this.Conditions.Parameters,
                Operations.DELETE
                );
        }
        
        public void CreateTable()
        {
            this.ExecuteNonQuery(
                $"{Operations.CREATE_TABLE.GetString()} {this.TableName} ({this.CreateColumns()});",
                this.NonConditions.Parameters,
                Operations.CREATE_TABLE
                );
        }

        public void AddTableColumns(params string[] columns)
        {
            if (columns.Count() == 0)
            {
                return;
            }

            this.ExecuteNonQuery(
                $"{Operations.ALTER_TABLE.GetString()} {this.TableName} {this.AddColumns(columns)};",
                this.NonConditions.Parameters,
                Operations.ALTER_TABLE
                );
        }
        
        public void DropTableColumns(params string[] columns)
        {
            if (columns.Count() == 0)
            {
                return;
            }

            this.ExecuteNonQuery(
                $"{Operations.ALTER_TABLE.GetString()} {this.TableName} {this.DropColumns(columns)};",
                this.NonConditions.Parameters,
                Operations.ALTER_TABLE
                );
        }

        public void Commit(Action<Schema<T>> action)
        {
            this.NonQuery = new NonQuery();
            this.NonQuery.Begin();
            
            action(new Schema<T>(this.TableName));

            if (this.NonQuery != null)
            {
                this.NonQuery.Commit();
            }
        }

        private int ExecuteNonQuery(string sql, Dictionary<string, object> parameters, Operations operations)
        {
            if (this.NonQuery == null)
            {
                var nonQuery = new NonQuery(sql, parameters);
                return nonQuery.ExecuteNonQuery(operations);
            }
            else
            {
                this.NonQuery.WriteSql(sql, parameters);
                return this.NonQuery.ExecuteNonQuery(operations);
            }
        }
    }

    public class Select<T> where T : Entity, new()
    {
        internal string TableName { get; set; }
        internal Schema<T> Schema { get; set; }
        internal List<JoinItem> Joined { get; set; } = new List<JoinItem>();

        /// <summary>
        /// Returns as dictionaries by the provied condition.
        /// </summary>
        public List<Dictionary<string, object>> Dictionaries
        {
            get
            {
                var dictionary = this.Schema.SelectBase().GetListDictionary();

                foreach (var item in dictionary)
                {
                    var usedKeys = new List<string>();

                    foreach (var property in typeof(T).GetProperties())
                    {
                        if (property.GetCustomAttribute<EncryptAttribute>(false) != null &&
                            item.ContainsKey(property.Name))
                        {
                            item[property.Name] = Encryption.Decrypt(Convert.ToString(item[property.Name]), true);
                            usedKeys.Add(property.Name);
                        }
                    }
                    
                    foreach (var joinedItem in Joined)
                    {
                        foreach (var property in joinedItem.EntityType.GetProperties())
                        {
                            int count = usedKeys.Count(x => x == property.Name);
                            string keyName = property.Name + (count == 0 ? null : (int?)count);

                            if (property.GetCustomAttribute<EncryptAttribute>(false) != null &&
                                item.ContainsKey(keyName))
                            {
                                item[keyName] = Encryption.Decrypt(Convert.ToString(item[keyName]), true);
                                usedKeys.Add(property.Name);
                            }
                        }
                    }
                }

                return dictionary;
            }
        }

        /// <summary>
        /// Returns the first dictionary by the provided condition.
        /// </summary>
        public Dictionary<string, object> Dictionary => this.Dictionaries.FirstOrDefault();

        /// <summary>
        /// Returns as list of entities by the provied condition.
        /// </summary>
        public List<T> Entities
        {
            get
            {
                return this.Dictionaries.Select(item => 
                {
                    return DictionaryClassConverter.DictionaryToClass<T>(item);
                })?.ToList();
            }
        }

        /// <summary>
        /// Returns the first entity by the provided condition.
        /// </summary>
        public T Entity => this.Entities.FirstOrDefault();
    }
}
