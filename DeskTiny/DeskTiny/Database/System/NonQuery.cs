using DTCore.DTSystem;
using DTCore.DTSystem.Diagnostics;
using DTCore.Tools;
using DTCore.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DTCore.Database.System
{
    public class NonQuery : Connect
    {
        public NonQuery(string sql, Dictionary<string, object> parameters) : base(sql, parameters) { }

        public NonQuery() : base()
        {
            this.HasMultipleNonQuery = true;
        }

        private bool HasMultipleNonQuery { get; set; } = false;

        public int ExecuteNonQuery(Operations operation)
        {
            if (this.NpgsqlTransaction == null)
            {
                this.NpgsqlConnection.Open();
            }
            
            var executionCount = this.NpgsqlCommand.ExecuteNonQuery();

            if (this.NpgsqlTransaction == null)
            {
                this.NpgsqlConnection.Close();
            }

            DTDebug.WriteLine($"{operation.GetString()} count", Convert.ToString(executionCount));

            if (new[] {
                Operations.ADD,
                Operations.ALTER_TABLE,
                Operations.CREATE_TABLE,
                Operations.DROP_COLUMN }.Contains(operation))
            {
                DTDebug.WriteLog(
                    ConfigurationBuilder.Logs.Migration,
                    $"Migration Details - {DateTime.Now}", 
                    CommandToSql.CommandAsSql(this.NpgsqlCommand));
            }
            
            return executionCount;
        }
        
        public void Begin()
        {
            if (this.NpgsqlConnection.State == ConnectionState.Closed)
            {
                this.NpgsqlConnection.Open();
            }
            
            this.NpgsqlTransaction = this.NpgsqlConnection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                this.NpgsqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                this.NpgsqlTransaction.Rollback();
                DTDebug.WriteLog(ex);
            }
            
            this.NpgsqlConnection.Close();
        }
    }
}
