using Npgsql;
using System;
using System.Collections.Generic;
using DeskTiny.Tools;

namespace DeskTiny.Database
{
    public class Connect
    {
        public NpgsqlConnection NpgsqlConnection { get; set; } = null;
        public NpgsqlCommand NpgsqlCommand { get; set; } = null;
        public NpgsqlDataReader NpgsqlDataReader { get; set; } = null;

        public Connect(string sql, Dictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new Exception("SQL not provided.");
            }

            string connectionString = ConfigurationBuilder.ConnectionString;
            
            Diagnostics.WriteDebug("Connecting database", connectionString);
            
            this.NpgsqlConnection = new NpgsqlConnection(connectionString);

            Diagnostics.WriteDebug("Executing query", sql);

            this.NpgsqlCommand = new NpgsqlCommand(sql, NpgsqlConnection);

            foreach (var parameter in parameters)
            {
                this.NpgsqlCommand.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
            }

            Diagnostics.WriteDebug("SQL Parameters", string.Join(Environment.NewLine, parameters));
        }
    }
}
