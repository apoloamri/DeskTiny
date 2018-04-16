using Tenderfoot.TfSystem;
using Tenderfoot.TfSystem.Diagnostics;
using Tenderfoot.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Tenderfoot.Database.System
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
            if (this.SqlTransaction == null)
            {
                this.SqlConnection.Open();
            }
            
            var executionCount = this.SqlCommand.ExecuteNonQuery();

            if (this.SqlTransaction == null)
            {
                this.SqlConnection.Close();
            }

            TfDebug.WriteLine($"{operation.GetString()} count", Convert.ToString(executionCount));

            if (new[] {
                Operations.ADD,
                Operations.ALTER_TABLE,
                Operations.CREATE_TABLE,
                Operations.DROP_COLUMN }.Contains(operation))
            {
                TfDebug.WriteLog(
                    Settings.Logs.Migration,
                    $"Migration Details - {DateTime.Now}",
                   this.SqlCommand.CommandText);
            }
            
            return executionCount;
        }
        
        public void Begin()
        {
            if (this.SqlConnection.State == ConnectionState.Closed)
            {
                this.SqlConnection.Open();
            }
            
            this.SqlTransaction = this.SqlConnection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                this.SqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                this.SqlTransaction.Rollback();
                TfDebug.WriteLog(ex);
            }
            
            this.SqlConnection.Close();
        }
    }
}
