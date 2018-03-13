using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DTCore.Tools;

namespace DTCore.Database.System
{
    public class Query : Connect
    {
        public Query(string sql, Dictionary<string, object> parameters) : base(sql, parameters) { }

        public List<DataRow> GetListDataRow()
        {
            this.NpgsqlConnection.Open();
            this.NpgsqlDataReader = this.NpgsqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();

            dataTable.Load(this.NpgsqlDataReader);

            this.NpgsqlConnection.Close();

            return dataTable?.Select()?.ToList();
        }

        public List<Dictionary<string, object>> GetListDictionary()
        {
            var data = this.GetListDataRow();

            return data.Select(item => {
                return item?
                    .Table?
                    .Columns?
                    .Cast<DataColumn>()?
                    .ToDictionary(x => x.ColumnName, x => item[x]);
            })?.ToList();
        }

        public List<Entity> GetListEntity<Entity>()
        {
            var data = this.GetListDictionary();

            return data.Select(item => {
                return DictionaryClassConverter.DictionaryToClass<Entity>(item);
            })?.ToList();
        }

        public long GetScalar()
        {
            this.NpgsqlConnection.Open();

            var scalar = Convert.ToInt64(this.NpgsqlCommand.ExecuteScalar());
            
            this.NpgsqlConnection.Close();

            return scalar;
        }
    }
}
