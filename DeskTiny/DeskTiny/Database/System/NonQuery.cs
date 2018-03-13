using DTCore.System.Diagnostics;
using DTCore.Tools.Extensions;
using System;
using System.Collections.Generic;

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

            return executionCount;
        }
    }
}
