using DTCore.System.Diagnostics;
using DTCore.Tools;
using DTCore.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTCore.Database.System
{
    public class NonQuery : Connect
    {
        public NonQuery(string sql, Dictionary<string, object> parameters) : base(sql, parameters) { }

        public int ExecuteNonQuery(Operations operation)
        {
            this.NpgsqlConnection.Open();
            
            var executionCount = this.NpgsqlCommand.ExecuteNonQuery();

            this.NpgsqlConnection.Close();

            Debug.WriteLine($"{operation.GetString()} count", Convert.ToString(executionCount));

            if (new[] {
                Operations.ADD,
                Operations.ALTER_TABLE,
                Operations.CREATE_TABLE,
                Operations.DROP_COLUMN }.Contains(operation))
            {
                Debug.WriteLog(
                    ConfigurationBuilder.Logs.Migration,
                    $"Migration Details - {DateTime.Now}", 
                    CommandToSql.CommandAsSql(this.NpgsqlCommand));
            }
            
            return executionCount;
        }

        public void BeginTransaction()
        {
            this.NpgsqlConnection.Open();
            this.NpgsqlTransaction = this.NpgsqlConnection.BeginTransaction();
        }

        public void Commit()
        {
            this.NpgsqlTransaction.Commit();
            this.NpgsqlConnection.Close();
        }
    }
}
