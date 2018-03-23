using DTCore.System;
using DTCore.System.Diagnostics;
using DTCore.Tools;
using DTCore.Tools.Extensions;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DTCore.Database.System
{
    public class Connect
    {
        public NpgsqlConnection NpgsqlConnection { get; set; } = null;
        public NpgsqlCommand NpgsqlCommand { get; set; } = null;
        public NpgsqlDataReader NpgsqlDataReader { get; set; } = null;
        public NpgsqlTransaction NpgsqlTransaction { get; set; } = null;

        public Connect(string sql, Dictionary<string, object> parameters)
        {
            if (sql.IsEmpty())
            {
                throw new CustomException("SQL not provided.");
            }

            string connectionString = ConfigurationBuilder.Database.ConnectionString;
            
            Debug.WriteLine("Connecting database", connectionString);
            
            this.NpgsqlConnection = new NpgsqlConnection(connectionString);

            Debug.WriteLine("Executing query", sql);

            this.NpgsqlCommand = new NpgsqlCommand(sql, NpgsqlConnection);

            foreach (var parameter in parameters)
            {
                this.NpgsqlCommand.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
            }

            Debug.WriteLine("SQL Parameters", string.Join(Environment.NewLine, parameters));
        }
    }
}
