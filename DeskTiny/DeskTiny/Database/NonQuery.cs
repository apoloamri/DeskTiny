using System;
using System.Collections.Generic;
using DeskTiny.Tools;

namespace DeskTiny.Database
{
    public class NonQuery : Connect
    {
        public NonQuery(string sql, Dictionary<string, object> parameters) : base(sql, parameters) { }

        public int ExecuteNonQuery(Operations operation)
        {
            this.NpgsqlConnection.Open();

            var executionCount = this.NpgsqlCommand.ExecuteNonQuery();

            this.NpgsqlConnection.Close();

            Diagnostics.WriteDebug($"{operation} count", Convert.ToString(executionCount));

            return executionCount;
        }
    }
}
